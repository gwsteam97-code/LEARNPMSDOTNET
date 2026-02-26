namespace PharmacyManagementSystem.Models;

public class Medicine
{
    public int Id { get; set; }

    public int PharmacyId { get; set; }   // ✅ REQUIRED

    public string Name { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}