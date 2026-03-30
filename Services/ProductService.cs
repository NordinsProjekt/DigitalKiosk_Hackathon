using Entities;
using Factories.Models;
using Services.Interfaces;

namespace Services;

public class ProductService(IProductRepository productRepository, IProductFactory productFactory) : IProductService
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
        Product product = productFactory.Create(productDetails);

        await productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(Guid id, ProductDetails productDetails)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        var validatedProduct = productFactory.Create(productDetails);
        product.Name = validatedProduct.Name;
        product.Description = validatedProduct.Description;
        product.ShelfLocation = validatedProduct.ShelfLocation;
        product.Section = validatedProduct.Section;
        product.Price = validatedProduct.Price;

        await productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await productRepository.DeleteAsync(id);
    }

    public async Task<List<Product>> FilterAsync(string query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        return await productRepository.FilterAsync(normalizedQuery);
    }
}
