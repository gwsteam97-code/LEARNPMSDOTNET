namespace PharmacyManagementSystem.Models;

public class Sale
{
    public int Id { get; set; }
    public int MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty; // joined from Medicines table
    public int Quantity { get; set; }
    public int PharmacyId { get; set; }
    public DateTime SaleDate { get; set; }    // <-- EF expects this column!
    public decimal TotalAmount { get; set; }  // optional
}
