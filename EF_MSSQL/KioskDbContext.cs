using Entities;
using Microsoft.EntityFrameworkCore;


namespace EF_MSSQL;

public class KioskDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<DiscountedProduct> DiscountedProducts { get; set; }
    public DbSet<CustomerDiscountedProduct> CustomerDiscountedProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        var connectionString = System.Environment.GetEnvironmentVariable("KIOSKDB_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var environmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.Equals(environmentName, "Development", System.StringComparison.OrdinalIgnoreCase))
            {
                // Development fallback to preserve out-of-the-box behavior for local runs and EF tooling
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=HackathonDb;Trusted_Connection=True;MultipleActiveResultSets=true;";
            }
            else
            {
                throw new InvalidOperationException(
                    "The database connection string is not configured. " +
                    "Please set the 'KIOSKDB_CONNECTION_STRING' environment variable to a valid SQL Server connection string.");
            }
        }
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KioskDbContext).Assembly);
    }
}
