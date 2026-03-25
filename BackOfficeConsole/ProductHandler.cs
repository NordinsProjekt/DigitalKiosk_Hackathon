using BackOfficeConsole.Validation;
using Entities;
using Entities.Enums;
using Services.Interfaces;

namespace BackOfficeConsole;

public class ProductHandler
{
    private readonly IProductService _productService;

    public ProductHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task AddProduct()
    {
        if (!ProductValidator.TryGetName(out var name)) return;
        if (!ProductValidator.TryGetDescription(out var description)) return;
        if (!ProductValidator.TryGetPrice(out var price)) return;
        if (!ProductValidator.TryGetShelfLocation(out var shelfLocation)) return;
        if (!ProductValidator.TryGetSection(out var section)) return;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            ShelfLocation = shelfLocation,
            Section = section
        };

       await _productService.AddAsync(product);
        Console.WriteLine("Produkt tillagd!");
    }

    public async Task ListProducts()
    {
        var products = await _productService.GetAllAsync();
        foreach (var p in products)
            Console.WriteLine($"[{p.Id}] {p.Name} - {p.Price:C}");
    }
}