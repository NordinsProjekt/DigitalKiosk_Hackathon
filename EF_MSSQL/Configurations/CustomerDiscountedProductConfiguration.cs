using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_MSSQL.Configurations;

public class CustomerDiscountedProductConfiguration : IEntityTypeConfiguration<CustomerDiscountedProduct>
{
    public void Configure(EntityTypeBuilder<CustomerDiscountedProduct> builder)
    {
        builder.HasKey(cdp => new { cdp.CustomerId, cdp.DiscountedProductId });

        builder.HasOne(cdp => cdp.Customer)
            .WithMany(c => c.CustomerDiscountedProducts)
            .HasForeignKey(cdp => cdp.CustomerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cdp => cdp.DiscountedProduct)
            .WithMany(dp => dp.CustomerDiscountedProducts)
            .HasForeignKey(cdp => cdp.DiscountedProductId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
