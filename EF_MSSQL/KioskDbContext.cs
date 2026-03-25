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
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=HackathonDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KioskDbContext).Assembly);
    }
}
