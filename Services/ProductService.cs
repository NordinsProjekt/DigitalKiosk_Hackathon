using Entities;
using Factories;
using Factories.Models;
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
            throw new KeyNotFoundException("Product not found.");

        product.Name = productDetails.Name;
        product.Description = productDetails.Description;
        product.ShelfLocation = productDetails.ShelfLocation;
        product.Section = productDetails.Section;
        product.Price = productDetails.Price;

        await productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await productRepository.DeleteAsync(id);
    }
}
