using Entities;
using Services.Interfaces;

namespace Services;

public class ProductService : IProductService
{
    public Task<List<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}