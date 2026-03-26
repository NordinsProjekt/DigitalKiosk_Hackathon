using Entities;
using Services.Interfaces;

namespace Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<List<Product>> GetAllAsync()
    {
        return await productRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await productRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        await productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await productRepository.DeleteAsync(id);
    }

    public async Task<List<Product>> FilterAsync(string query)
    {
        var normalizedQuery = query?.Trim();

        return await productRepository.FilterAsync(normalizedQuery!);
    }
}