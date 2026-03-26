using Entities;

namespace Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(ProductDetails productDetails);
    Task UpdateAsync(Guid id, ProductDetails productDetails);
    Task DeleteAsync(Guid id);
}