using Entities;

namespace Services.Interfaces;

public interface IDiscountedProductsRepository
{
    Task<List<DiscountedProduct>> GetAllAsync();
    Task<DiscountedProduct?> GetAsync(Guid id);
    Task AddAsync(DiscountedProduct discountedProduct);
    Task UpdateAsync(DiscountedProduct discountedProduct);
    Task DeleteAsync(Guid id);
}