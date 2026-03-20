using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace EF_MSSQL.Repositories;

public class CustomerDiscountedProductRepository(KioskDbContext context) : ICustomerDiscountedProductRepository
{
    public async Task<List<CustomerDiscountedProduct>> GetAllAsync()
    {
        return await context.CustomerDiscountedProducts.AsNoTracking().ToListAsync();
    }

    public async Task<CustomerDiscountedProduct?> GetByIdAsync(Guid id)
    {
        return await context.CustomerDiscountedProducts.FindAsync(id);
    }

    public async Task AddAsync(CustomerDiscountedProduct discountedProduct)
    {
        await context.CustomerDiscountedProducts.AddAsync(discountedProduct);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerDiscountedProduct discountedProduct)
    {
        context.CustomerDiscountedProducts.Update(discountedProduct);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var discountedProduct = await context.CustomerDiscountedProducts.FindAsync(id);
        if (discountedProduct == null) return;
        
        context.CustomerDiscountedProducts.Remove(discountedProduct);
        await context.SaveChangesAsync();
    }
}