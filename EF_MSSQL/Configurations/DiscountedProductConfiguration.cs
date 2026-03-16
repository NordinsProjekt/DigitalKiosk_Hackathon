using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_MSSQL.Configurations;

public class DiscountedProductConfiguration : IEntityTypeConfiguration<DiscountedProduct>
{
    public void Configure(EntityTypeBuilder<DiscountedProduct> builder)
    {
        builder.HasKey(dp => dp.Id);

        builder.Property(dp => dp.Discount)
            .IsRequired();

        builder.Property(dp => dp.MaximumProducts)
            .IsRequired();

        builder.HasOne(dp => dp.Product)
            .WithMany(p => p.DiscountedProducts)
            .HasForeignKey(dp => dp.ProductId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
