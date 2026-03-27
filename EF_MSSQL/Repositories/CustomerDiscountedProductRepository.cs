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

    public async Task<CustomerDiscountedProduct?> GetByIdAsync(Guid discontinuedProductId, Guid customerId)
    {
        return await context.CustomerDiscountedProducts.FindAsync(discontinuedProductId, customerId);
    }

    public async Task AddAsync(CustomerDiscountedProduct customerDiscountedProduct)
    {
        await context.CustomerDiscountedProducts.AddAsync(customerDiscountedProduct);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerDiscountedProduct customerDiscountedProduct)
    {
        context.CustomerDiscountedProducts.Update(customerDiscountedProduct);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid discontinuedProductId, Guid customerId)
    {
        var customerDiscountedProduct = await context.CustomerDiscountedProducts.FindAsync(discontinuedProductId, customerId);
        if (customerDiscountedProduct == null) return;

        context.CustomerDiscountedProducts.Remove(customerDiscountedProduct);
        await context.SaveChangesAsync();
    }

}
