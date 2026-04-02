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
            () => inner.GetAllAsync(),
            payloadType: "DiscountedProduct[]");
    }

    public Task<DiscountedProduct?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "GetByIdAsync",
            "DiscountedProductRepository", "GetByIdAsync",
            "Repository",
            () => inner.GetByIdAsync(id),
            input: new { id }, payloadType: "DiscountedProduct");
    }

    public Task AddAsync(DiscountedProduct discountedProduct)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "AddAsync",
            "DiscountedProductRepository", "AddAsync",
            "Repository",
            () => inner.AddAsync(discountedProduct),
            input: discountedProduct, payloadType: "DiscountedProduct");
    }

    public Task UpdateAsync(DiscountedProduct discountedProduct)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "UpdateAsync",
            "DiscountedProductRepository", "UpdateAsync",
            "Repository",
            () => inner.UpdateAsync(discountedProduct),
            input: discountedProduct, payloadType: "DiscountedProduct");
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "DiscountedProductService", "DeleteAsync",
            "DiscountedProductRepository", "DeleteAsync",
            "Repository",
            () => inner.DeleteAsync(id),
            input: new { id }, payloadType: "Guid");
    }
}
