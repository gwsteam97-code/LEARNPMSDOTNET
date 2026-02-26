using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Services.Interfaces;

public interface IMedicineService
{
    Task<IEnumerable<Medicine>> GetAllAsync();
    Task<Medicine?> GetByIdAsync(int id);
    Task<(bool success, string message, Medicine? data)> AddAsync(Medicine medicine);
    Task<Medicine?> UpdateAsync(int id, Medicine medicine);
    Task<bool> DeleteAsync(int id);
}