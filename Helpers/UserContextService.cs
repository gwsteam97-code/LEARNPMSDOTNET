using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PharmacyManagementSystem.Services
{
    public interface IUserContextService
    {
        int GetPharmacyId();
        string GetUserEmail(); // optional, if you want
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetPharmacyId()
        {
            var pharmacyIdClaim = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst("PharmacyId")?.Value;

            if (string.IsNullOrEmpty(pharmacyIdClaim))
                throw new Exception("Unauthorized Access");

            return Convert.ToInt32(pharmacyIdClaim);
        }

        public string GetUserEmail()
        {
            var email = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new Exception("Unauthorized Access");

            return email;
        }
    }
}