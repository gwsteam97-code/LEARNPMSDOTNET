using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PharmacyManagementSystem.Services.Implementations;

public class MedicineService : IMedicineService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public MedicineService(
        AppDbContext context,
        IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    // ===============================
    // GET PHARMACY ID FROM TOKEN
    // ===============================
    private int GetPharmacyId()
    {
        var pharmacyId = _httpContext.HttpContext?
            .User?
            .FindFirst("PharmacyId")?.Value;

        if (string.IsNullOrEmpty(pharmacyId))
            throw new Exception("Unauthorized Access");

        return Convert.ToInt32(pharmacyId);
    }

    // ===============================
    // GET ALL MEDICINES
    // ===============================
    public async Task<IEnumerable<Medicine>> GetAllAsync()
    {
        int pharmacyId = GetPharmacyId();

        var medicines = await _context.Medicines
            .FromSqlRaw(
                "EXEC sp_GetAllMedicines @PharmacyId",
                new SqlParameter("@PharmacyId", pharmacyId))
            .ToListAsync();

        return medicines;
    }

    // ===============================
    // GET BY ID
    // ===============================
    public async Task<Medicine?> GetByIdAsync(int id)
    {
        int pharmacyId = GetPharmacyId();

        return await _context.Medicines
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.PharmacyId == pharmacyId);
    }

    // ===============================
    // ADD MEDICINE
    // ===============================
    public async Task<(bool success, string message, Medicine? data)>
        AddAsync(Medicine medicine)
    {
        int pharmacyId = GetPharmacyId();

        using var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "sp_AddMedicine";
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter("@PharmacyId", pharmacyId));
        command.Parameters.Add(new SqlParameter("@Name", medicine.Name));
        command.Parameters.Add(new SqlParameter("@BatchNumber", medicine.BatchNumber));
        command.Parameters.Add(new SqlParameter("@ExpiryDate", medicine.ExpiryDate));
        command.Parameters.Add(new SqlParameter("@Price", medicine.Price));
        command.Parameters.Add(new SqlParameter("@Stock", medicine.Stock));

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var status = Convert.ToInt32(reader["Status"]);
            var message = reader["Message"]?.ToString();

            if (status == 0)
                return (false, message!, null);

            var result = new Medicine
            {
                Id = Convert.ToInt32(reader["Id"]),
                PharmacyId = pharmacyId,
                Name = medicine.Name,
                BatchNumber = medicine.BatchNumber,
                ExpiryDate = medicine.ExpiryDate,
                Price = medicine.Price,
                Stock = medicine.Stock
            };

            return (true, message!, result);
        }

        return (false, "Unknown error", null);
    }

    // ===============================
    // UPDATE MEDICINE
    // ===============================
    public async Task<Medicine?> UpdateAsync(int id, Medicine medicine)
    {
        int pharmacyId = GetPharmacyId();

        var existing = await _context.Medicines
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.PharmacyId == pharmacyId);

        if (existing == null)
            return null;

        existing.Name = medicine.Name;
        existing.BatchNumber = medicine.BatchNumber;
        existing.ExpiryDate = medicine.ExpiryDate;
        existing.Price = medicine.Price;
        existing.Stock = medicine.Stock;

        await _context.SaveChangesAsync();

        return existing;
    }

    // ===============================
    // DELETE MEDICINE
    // ===============================
    public async Task<bool> DeleteAsync(int id)
    {
        int pharmacyId = GetPharmacyId();

        var medicine = await _context.Medicines
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.PharmacyId == pharmacyId);

        if (medicine == null)
            return false;

        _context.Medicines.Remove(medicine);
        await _context.SaveChangesAsync();

        return true;
    }
}