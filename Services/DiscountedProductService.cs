using Entities;
using Services.Interfaces;

namespace Services;

public class DiscountedProductService(IDiscountedProductRepository repo) : IDiscountedProductService
{
    public async Task<List<DiscountedProduct>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<DiscountedProduct?> GetByIdAsync(Guid id)
    {
        return await repo.GetByIdAsync(id);
    }

    public async Task AddAsync(DiscountedProduct discountedProduct)
    {
        await repo.AddAsync(discountedProduct);
    }

    public async Task UpdateAsync(DiscountedProduct discountedProduct)
    {
        await repo.UpdateAsync(discountedProduct);
    }

    public async Task DeleteAsync(Guid id)
    {
        await repo.DeleteAsync(id);
    }
}