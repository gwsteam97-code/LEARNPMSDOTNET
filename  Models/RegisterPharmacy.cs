namespace PharmacyManagementSystem.Models;

public class RegisterPharmacy
{
    public int Id { get; set; }

    public string PharmacyName { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}