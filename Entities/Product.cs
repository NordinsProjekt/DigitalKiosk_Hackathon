using Entities.Enums;

namespace Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShelfLocation ShelfLocation { get; set; }
    public Section Section { get; set; }
    public decimal Price { get; set; }

    public ICollection<DiscountedProduct> DiscountedProducts { get; set; } = [];
}
