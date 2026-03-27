using BackOfficeConsole.Validation;
using Entities;
using Services.Interfaces;

namespace BackOfficeConsole;

public class ProductHandler
{
    private readonly IProductService _productService;

    public ProductHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task EditProductAsync()
    {
        var products = await _productService.GetAllAsync();
        if (!products.Any())
        {
            Console.WriteLine("Inga produkter finns i systemet.");
            return;
        }
        Console.WriteLine("\n=== Välj produkt att redigera ===");
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {products[i].Name} | {products[i].Price:C} | {products[i].Section} {products[i].ShelfLocation}");
        }

        Console.WriteLine("\n Ange nummer: ");
        if (!int.TryParse(Console.ReadLine(), out var productChoice)
            || productChoice < 1 || productChoice > products.Count)
        {
            Console.WriteLine("Ogitigt val!");
            return;
        }
        var product = products[productChoice - 1];

        Console.WriteLine($"\nRedigerar: {product.Name}");
        Console.WriteLine("1.Ändra beskrivning");
        Console.WriteLine("2.Ändra pris");
        Console.WriteLine("3.Flytta hyllplats");
        Console.WriteLine("4.Avbryt");
        Console.Write("\n Gör ett val: ");

        if (!int.TryParse(Console.ReadLine(), out int menuChoice))
        {
            Console.WriteLine("Ogiltigt val!");
            return;
        }

        switch (menuChoice)
        {
            case 1:
                if (!ProductValidator.TryGetDescription(out var desc))
                    return;
                product.Description = desc;
                break;

            case 2:
                if (!ProductValidator.TryGetPrice(out var newPrice))
                    return;
                product.Price = newPrice;
                break;

            case 3:
                if (!ProductValidator.TryGetShelfLocation(out var newLocation))
                    return;
                product.ShelfLocation = newLocation;
                break;

            case 4:
                Console.WriteLine("Avbrutet.");
                return;

            default:
                Console.WriteLine("Ogiltigt val.");
                return;
        }
        await _productService.UpdateAsync(product);
        Console.WriteLine("✓ Produkten har uppdaterats!");


    }

    public async Task AddProduct()
    {
        if (!ProductValidator.TryGetName(out var name)) return;
        if (!ProductValidator.TryGetDescription(out var description)) return;
        if (!ProductValidator.TryGetPrice(out var price)) return;
        if (!ProductValidator.TryGetSection(out var section)) return;
        if (!ProductValidator.TryGetShelfLocation(out var shelfLocation)) return;

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
        Console.WriteLine("Laddar...");
        var products = await _productService.GetAllAsync();
        Console.Clear();
        foreach (var p in products.OrderBy(p => p.Price))
            Console.WriteLine($"{p.Name} - {p.Price:C}");
    }
}