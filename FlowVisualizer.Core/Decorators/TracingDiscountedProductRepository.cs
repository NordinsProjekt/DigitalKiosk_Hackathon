using Entities;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingDiscountedProductRepository(IDiscountedProductRepository inner, FlowTracer tracer) : IDiscountedProductRepository
{
    public Task<List<DiscountedProduct>> GetAllAsync()
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "GetAllAsync",
            "DiscountedProductRepository", "GetAllAsync",
            "Repository",
            () => inner.GetAllAsync());
    }

    public Task<DiscountedProduct?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "GetByIdAsync",
            "DiscountedProductRepository", "GetByIdAsync",
            "Repository",
            () => inner.GetByIdAsync(id));
    }

    public Task AddAsync(DiscountedProduct discountedProduct)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "AddAsync",
            "DiscountedProductRepository", "AddAsync",
            "Repository",
            () => inner.AddAsync(discountedProduct));
    }

    public Task UpdateAsync(DiscountedProduct discountedProduct)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "UpdateAsync",
            "DiscountedProductRepository", "UpdateAsync",
            "Repository",
            () => inner.UpdateAsync(discountedProduct));
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "DeleteAsync",
            "DiscountedProductRepository", "DeleteAsync",
            "Repository",
            () => inner.DeleteAsync(id));
    }
}
