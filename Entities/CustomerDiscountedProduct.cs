namespace Entities;

public class CustomerDiscountedProduct
{
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid? DiscountedProductId { get; set; }
    public DiscountedProduct? DiscountedProduct { get; set; }
}
