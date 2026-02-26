public class Supplier
{
    public int Id { get; set; }
    public int? PharmacyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty; // Phone number
    public string? Email { get; set; } // nullable
    public string? Address { get; set; } // nullable
    public DateTime CreatedAt { get; set; }
}