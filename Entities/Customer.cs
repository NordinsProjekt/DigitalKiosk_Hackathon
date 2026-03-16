namespace Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string PersonalIdentityNumber { get; set; } = null!;

    public ICollection<CustomerDiscountedProduct> CustomerDiscountedProducts { get; set; } = new List<CustomerDiscountedProduct>();
}
