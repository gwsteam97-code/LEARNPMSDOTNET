using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.DTOs.Auth;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // =====================
    // REGISTER PHARMACY
    // =====================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterPharmacyDto dto)
    {
        var result = await _authService.Register(dto);
        return Ok(result);
    }

    // =====================
    // LOGIN
    // =====================
    [HttpPost("login")]
    public IActionResult Login(string email, string password)
    {
        var token = _authService.Login(email, password);
        return Ok(new { token });
    }
}