using PharmacyManagementSystem.DTOs.Auth;

namespace PharmacyManagementSystem.Services.Interfaces;

public interface IAuthService
{
    Task<string> Register(RegisterPharmacyDto dto);

    string Login(string email, string password);
}