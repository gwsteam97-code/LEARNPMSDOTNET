using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Services.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetAllAsync();

    Task<(bool success, string message, Supplier? data)>
        AddAsync(Supplier supplier);
}