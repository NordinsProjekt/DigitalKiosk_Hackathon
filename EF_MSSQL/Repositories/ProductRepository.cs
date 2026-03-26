using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace EF_MSSQL.Repositories;

public class ProductRepository(KioskDbContext context) : IProductRepository
{
    public async Task<List<Product>> GetAllAsync()
    {
        return await context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return;

        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task<List<Product>> FilterAsync(string query)
    {

        var filter = await context.Products

            .AsNoTracking()

            .Where(x => x.Name.ToLower().Contains(query))
            
            .ToListAsync();

        return filter;
    }
}