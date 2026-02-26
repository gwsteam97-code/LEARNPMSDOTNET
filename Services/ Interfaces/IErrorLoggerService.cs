using System;
using System.Threading.Tasks;

namespace PharmacyManagementSystem.Services.Interfaces
{
    public interface IErrorLoggerService
    {
        Task LogAsync(Exception ex, int? pharmacyId = null);
    }
}
