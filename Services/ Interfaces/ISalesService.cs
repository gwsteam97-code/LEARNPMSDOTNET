using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Services.Interfaces;

public interface ISalesService
{
    Task<IEnumerable<Sale>> GetAllAsync();

    Task<(bool success, string message, Sale? data)>
        CreateSaleAsync(int medicineId, int quantity);
}