using Entities.Enums;

namespace Entities;

public class Product
{
    public Product(string name, string description, ShelfLocation shelfLocation, Section section, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ShelfLocation = shelfLocation;
        Section = section;
        Price = price;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShelfLocation ShelfLocation { get; set; }
    public Section Section { get; set; }
    public decimal Price { get; set; }
    public ICollection<DiscountedProduct> DiscountedProducts { get; set; } = [];
}
