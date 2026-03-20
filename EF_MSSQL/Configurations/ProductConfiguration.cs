using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_MSSQL.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.ShelfLocation)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(p => p.Section)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
