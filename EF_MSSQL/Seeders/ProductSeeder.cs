using Entities;
using Entities.Enums;

namespace EF_MSSQL.Seeders;

public static class ProductSeeder
{
    private static readonly Random Rng = new(42);
    
    private static readonly string[] Adjectives =
    [
        "Premium", "Deluxe", "Classic", "Pro", "Ultra", "Mini", "Maxi", "Smart",
        "Eco", "Flex", "Rapid", "Heavy-Duty", "Lite", "Advanced", "Essential",
        "Compact", "Robust", "Portable", "Wireless", "Ergonomic"
    ];

    private static readonly string[] Nouns =
    [
        "Wrench", "Drill", "Hammer", "Screwdriver", "Pliers", "Saw", "Level",
        "Tape Measure", "Chisel", "Sander", "Grinder", "Router", "Clamp",
        "Nail Gun", "Stapler", "Caulk Gun", "Heat Gun", "Voltage Tester",
        "Socket Set", "Hex Key", "Bolt", "Nut", "Washer", "Anchor", "Hook",
        "Bracket", "Rail", "Panel", "Fitting", "Valve", "Coupling", "Elbow",
        "Tee", "Cap", "Hose", "Cable", "Wire", "Connector", "Terminal",
        "Switch", "Relay", "Fuse", "Circuit Breaker", "Conduit", "Junction Box",
        "Shelf", "Cabinet", "Drawer", "Bin", "Container", "Rack", "Peg",
        "Trolley", "Cart", "Dolly", "Pallet", "Crate", "Ladder", "Scaffold"
    ];

    private static readonly string[] Materials =
    [
        "Steel", "Aluminium", "Carbon Fibre", "Titanium", "Brass", "Copper",
        "PVC", "ABS", "Nylon", "Rubber", "Ceramic", "Stainless"
    ];

    private static readonly string[] Sizes = ["XS", "S", "M", "L", "XL", "XXL", "3XL"];
    
    private static readonly string[] DescriptionTemplates =
    [
        "High-quality {material} construction for lasting durability.",
        "Designed for both professional and home use.",
        "Ergonomic grip reduces fatigue during extended use.",
        "Meets industry safety standards and certifications.",
        "Lightweight yet exceptionally strong.",
        "Ideal for heavy-duty applications in demanding environments.",
        "Corrosion-resistant finish extends product lifespan.",
        "Precision-engineered for accurate, repeatable results.",
        "Quick-release mechanism for effortless operation.",
        "Compatible with standard fittings and accessories.",
        "Available in multiple configurations to suit your needs.",
        "Backed by a manufacturer's quality guarantee.",
        "Easy to clean and maintain.",
        "Compact design fits in tight spaces.",
        "Energy-efficient performance reduces operating costs."
    ];
    
    private static readonly ShelfLocation[] ShelfLocations =
        Enum.GetValues<ShelfLocation>();

    private static readonly Section[] Sections =
        Enum.GetValues<Section>();
    
    public static List<Product> Generate(int count = 3000)
    {
        var products = new List<Product>(count);
        var usedNames = new HashSet<string>();

        while (products.Count < count)
        {
            var name = BuildName();
            if (!usedNames.Add(name)) continue; 
            products.Add(new Product
            {
                Id              = Guid.NewGuid(),
                Name            = name,
                Description     = BuildDescription(),
                ShelfLocation   = ShelfLocations[Rng.Next(ShelfLocations.Length)],
                Section         = Sections[Rng.Next(Sections.Length)],
                Price           = BuildPrice(),
            });
        }

        return products;
    }
    
    private static string BuildName()
    {
        return $"{Pick(Adjectives)} {Pick(Materials)} {Pick(Nouns)} {Pick(Sizes)}";
    }

    private static string BuildDescription()
    {
        int count = Rng.Next(2, 4);
        var sentences = DescriptionTemplates
            .OrderBy(_ => Rng.Next())
            .Take(count)
            .Select(t => t.Replace("{material}", Pick(Materials)));
        return string.Join(" ", sentences);
    }

    private static decimal BuildPrice()
    {
        double raw = Rng.NextDouble() * 2490 + 10;
        return Math.Round((decimal)raw, 2);
    }

    private static T Pick<T>(T[] array) => array[Rng.Next(array.Length)];
}