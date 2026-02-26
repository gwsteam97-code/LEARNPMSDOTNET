using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _userContext;

    public SupplierService(AppDbContext context, IUserContextService userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    // ✅ Get All Suppliers for logged-in pharmacy
    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        int pharmacyId = _userContext.GetPharmacyId();

        return await _context.Suppliers
            .FromSqlRaw("EXEC sp_GetAllSuppliers @PharmacyId",
                new SqlParameter("@PharmacyId", pharmacyId))
            .ToListAsync();
    }

    // ✅ Add Supplier using SP, PharmacyId from token
    public async Task<(bool success, string message, Supplier? data)> AddAsync(Supplier supplier)
    {
        using var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "sp_AddSupplier";
        command.CommandType = CommandType.StoredProcedure;

        // Assign PharmacyId from token
        supplier.PharmacyId = _userContext.GetPharmacyId();

        command.Parameters.Add(new SqlParameter("@Name", supplier.Name));
        command.Parameters.Add(new SqlParameter("@ContactNumber", supplier.ContactNumber));
        command.Parameters.Add(new SqlParameter("@Email", (object?)supplier.Email ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Address", (object?)supplier.Address ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PharmacyId", supplier.PharmacyId));

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            int status = Convert.ToInt32(reader["Status"]);
            string message = reader["Message"].ToString()!;

            if (status == 0)
                return (false, message, null);

            supplier.Id = Convert.ToInt32(reader["Id"]);

            return (true, message, supplier);
        }

        return (false, "Unknown error occurred", null);
    }
}