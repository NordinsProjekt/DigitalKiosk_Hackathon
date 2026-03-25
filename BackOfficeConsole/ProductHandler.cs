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