using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.DTOs.Auth;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context,
                       IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // =============================
    // REGISTER PHARMACY
    // =============================
    public async Task<string> Register(RegisterPharmacyDto dto)
    {
        // Check already exists
        var exists = await _context.RegisterPharmacies
            .AnyAsync(x => x.Email == dto.Email);
        if (exists)
            throw new Exception("Pharmacy already registered");

        var pharmacy = new RegisterPharmacy
        {
            PharmacyName = dto.PharmacyName,
            OwnerName = dto.OwnerName,
            Email = dto.Email,
            Password = dto.Password,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTime.Now
        };

        _context.RegisterPharmacies.Add(pharmacy);
        await _context.SaveChangesAsync();
        return "Pharmacy Registered Successfully";
    }

    // =============================
    // LOGIN
    // =============================
    public string Login(string email, string password)
    {
        var pharmacy = _context.RegisterPharmacies
            .FirstOrDefault(x =>
                x.Email == email &&
                x.Password == password);

        if (pharmacy == null)
            throw new Exception("Invalid Email or Password");

        return GenerateToken(pharmacy);
    }

    // =============================
    // JWT TOKEN GENERATION
    // =============================
    private string GenerateToken(RegisterPharmacy pharmacy)
    {
        var claims = new[]
        {
            new Claim("PharmacyId", pharmacy.Id.ToString()),
            new Claim("PharmacyName", pharmacy.PharmacyName),
            new Claim(ClaimTypes.Email, pharmacy.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }


}
