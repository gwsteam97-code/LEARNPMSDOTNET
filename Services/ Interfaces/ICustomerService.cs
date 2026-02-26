using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> AddAsync(Customer customer);
}