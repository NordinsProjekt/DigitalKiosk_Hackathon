using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace EF_MSSQL.Repositories;

public class DiscountedProductRepository(KioskDbContext context) : IDiscountedProductRepository
{
    public async Task<List<DiscountedProduct>> GetAllAsync()
    {
        return await context.DiscountedProducts.AsNoTracking().ToListAsync();
    }

    public async Task<DiscountedProduct?> GetByIdAsync(Guid discontinuedProductId, Guid customerId)
    {
        return await context.DiscountedProducts.FindAsync(discontinuedProductId, customerId);
    }

    public async Task AddAsync(DiscountedProduct discountedProduct)
    {
        await context.DiscountedProducts.AddAsync(discountedProduct);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DiscountedProduct discountedProduct)
    {
        context.DiscountedProducts.Update(discountedProduct);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid discontinuedProductId, Guid customerId)
    {
        var discountedProduct = await context.DiscountedProducts.FindAsync(discontinuedProductId, customerId);
        if (discountedProduct == null) return;
        
        context.DiscountedProducts.Remove(discountedProduct);
        await context.SaveChangesAsync();
    }
    
}