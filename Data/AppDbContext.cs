using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<RegisterPharmacy> RegisterPharmacies { get; set; }
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Log> Logs { get; set; }
    public DbSet<Sale> Sales => Set<Sale>();
}