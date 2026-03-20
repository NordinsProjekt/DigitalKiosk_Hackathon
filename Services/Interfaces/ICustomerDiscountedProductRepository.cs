using Entities;

namespace Services.Interfaces;

public interface ICustomerDiscountedProductRepository
{
    Task<List<DiscountedProduct>> GetAllAsync();
    Task<DiscountedProduct> GetByIdAsync(Guid id);
    Task AddAsync(CustomerDiscountedProduct customerDiscountedProduct);
    Task UpdateAsync(CustomerDiscountedProduct customerDiscountedProduct);
    Task DeleteAsync(Guid id);
    
}