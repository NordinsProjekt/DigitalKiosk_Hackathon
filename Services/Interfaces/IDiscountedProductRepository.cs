using Entities;

namespace Services.Interfaces;

public interface IDiscountedProductRepository
{
    Task<List<DiscountedProduct>> GetAllAsync();
    Task<DiscountedProduct?> GetByIdAsync(Guid id, Guid customerId);
    Task AddAsync(DiscountedProduct discountedProduct);
    Task UpdateAsync(DiscountedProduct discountedProduct);
    Task DeleteAsync(Guid id, Guid customerId);
    
}