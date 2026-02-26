using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PharmacyManagementSystem.Services.Implementations
{
    public class ErrorLoggerService : IErrorLoggerService
    {
        private readonly AppDbContext _context;

        public ErrorLoggerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(Exception ex, int? pharmacyId = null)
        {
            try
            {
                var log = new Log
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    PharmacyId = pharmacyId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Avoid throwing another exception if logging fails
            }
        }
    }
}