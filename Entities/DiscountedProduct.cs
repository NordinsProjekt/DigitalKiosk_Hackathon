namespace Entities;

public class DiscountedProduct
{
    public Guid Id { get; set; }
    public int Discount { get; set; }
    public int MaximumProducts { get; set; }

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }

    public ICollection<CustomerDiscountedProduct> CustomerDiscountedProducts { get; set; } = [];
}
