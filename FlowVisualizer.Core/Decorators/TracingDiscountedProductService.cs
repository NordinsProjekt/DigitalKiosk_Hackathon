using Entities;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingDiscountedProductService(IDiscountedProductService inner, FlowTracer tracer) : IDiscountedProductService
{
    public Task<List<DiscountedProduct>> GetAllAsync()
    {
        return tracer.TraceAsync(
            "DiscountedProductController", "GetAll",
            "DiscountedProductService", "GetAllAsync",
            "Service",
            () => inner.GetAllAsync(),
            payloadType: "DiscountedProduct[]");
    }

    public Task<DiscountedProduct?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "DiscountedProductController", "GetById",
            "DiscountedProductService", "GetByIdAsync",
            "Service",
            () => inner.GetByIdAsync(id),
            input: new { id }, payloadType: "DiscountedProduct");
    }

    public Task AddAsync(DiscountedProduct discountedProduct)
    {
        return tracer.TraceAsync(
            "DiscountedProductController", "Add",
            "DiscountedProductService", "AddAsync",
            "Service",
            () => inner.AddAsync(discountedProduct),
            input: discountedProduct, payloadType: "DiscountedProduct");
    }

    public Task UpdateAsync(DiscountedProduct discountedProduct)
    {
        return tracer.TraceAsync(
            "DiscountedProductController", "Update",
            "DiscountedProductService", "UpdateAsync",
            "Service",
            () => inner.UpdateAsync(discountedProduct),
            input: discountedProduct, payloadType: "DiscountedProduct");
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "DiscountedProductController", "Delete",
            "DiscountedProductService", "DeleteAsync",
            "Service",
            () => inner.DeleteAsync(id),
            input: new { id }, payloadType: "Guid");
    }
}
