using Entities;
using Factories;
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

    public async Task AddAsync(ProductDetails productDetails)
    {
        Product product = ProductFactory.Create(productDetails);

        await productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(Guid id, ProductDetails productDetails)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            throw new Exception("Product not found.");

        var updated = ProductFactory.Create(productDetails);

        updated.Id = product.Id;

        await productRepository.UpdateAsync(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        await productRepository.DeleteAsync(id);
    }
}
