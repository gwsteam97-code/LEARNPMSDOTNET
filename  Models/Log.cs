namespace PharmacyManagementSystem.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string? StackTrace { get; set; }
        public int? PharmacyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}