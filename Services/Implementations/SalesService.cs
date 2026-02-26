using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Services.Implementations;

public class SalesService : ISalesService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _userContext;
    private readonly IErrorLoggerService _errorLogger;

    // Constructor with all dependencies
    public SalesService(
        AppDbContext context,
        IUserContextService userContext,
        IErrorLoggerService errorLogger)
    {
        _context = context;
        _userContext = userContext;
        _errorLogger = errorLogger;
    }

    // ✅ Get All Sales using SP with exception handling
    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        int pharmacyId = _userContext.GetPharmacyId();
        try
        {
            return await _context.Sales
                .FromSqlRaw(
                    "EXEC sp_GetAllSales @PharmacyId",
                    new SqlParameter("@PharmacyId", pharmacyId))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log error to database
            await _errorLogger.LogAsync(ex, pharmacyId);
            return new List<Sale>(); // Return empty list on error
        }
    }

    // ✅ Create Sale using SP with exception handling
    public async Task<(bool success, string message, Sale? data)> CreateSaleAsync(int medicineId, int quantity)
    {
        int pharmacyId = _userContext.GetPharmacyId();

        try
        {
            // Test error for logging
            // if (medicineId == -1)
            //     throw new Exception("Test error during sale creation");

            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_CreateSale";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@MedicineId", medicineId));
            command.Parameters.Add(new SqlParameter("@Quantity", quantity));
            command.Parameters.Add(new SqlParameter("@PharmacyId", pharmacyId));

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                int status = Convert.ToInt32(reader["Status"]);
                string message = reader["Message"].ToString()!;

                if (status == 0)
                    return (false, message, null);

                var sale = new Sale
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MedicineId = Convert.ToInt32(reader["MedicineId"]),
                    MedicineName = reader["MedicineName"]?.ToString() ?? string.Empty,
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    PharmacyId = pharmacyId,
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    SaleDate = Convert.ToDateTime(reader["SaleDate"])
                };

                // Optionally save to local DbContext
                //_context.Sales.Add(sale);
                //await _context.SaveChangesAsync();

                return (true, message, sale);
            }

            return (false, "Sale failed", null);
        }
        catch (Exception ex)
        {
            await _errorLogger.LogAsync(ex, pharmacyId);
            return (false, "An error occurred during sale creation", null);
        }
    }
}
