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
                Console.Write($"Nuvarande beskrivning: {product.Description}\nNy beskrivning: ");
                var desc = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(desc))
                {
                    Console.WriteLine("Beskrivning får inte vara tom.");
                    return;
                }
                product.Description = desc;
                break;

            case 2:
                Console.Write($"Nuvarande pris: {product.Price:C}\nNytt pris: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice) || newPrice <= 0)
                {
                    Console.WriteLine("Ogiltigt pris. Måste vara ett tal som är större än 0.");
                    return;
                }
                product.Price = newPrice;
                break;

            case 3:
                Console.WriteLine("Välj ny hyllplats:");
                var locations = Enum.GetValues<ShelfLocation>();
                for (int i = 0; i < locations.Length; i++)
                    Console.WriteLine($"{i + 1}. {locations[i]}");

                Console.Write("Ange nummer: ");
                if (!int.TryParse(Console.ReadLine(), out int locChoice)
                    || locChoice < 1 || locChoice > locations.Length)
                {
                    Console.WriteLine("Ogiltigt val.");
                    return;
                }
                product.ShelfLocation = locations[locChoice - 1];
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
        Console.Write("Namn: ");
        var name = Console.ReadLine()!;

        Console.Write("Beskrivning: ");
        var description = Console.ReadLine()!;

        Console.Write("Pris: ");
        decimal.TryParse(Console.ReadLine(), out decimal price);

        Console.WriteLine("Hyllplats:");
        foreach (var s in Enum.GetValues<ShelfLocation>())
            Console.WriteLine($"  {(int)s}. {s}");
        Console.Write("Välj: ");
        var shelfLocation = (ShelfLocation)int.Parse(Console.ReadLine()!);

        Console.WriteLine("Sektion:");
        foreach (var s in Enum.GetValues<Section>())
            Console.WriteLine($"  {(int)s}. {s}");
        Console.Write("Välj: ");
        var section = (Section)int.Parse(Console.ReadLine()!);

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