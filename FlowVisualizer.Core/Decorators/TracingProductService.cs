using Entities;
using Factories.Models;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingProductService(IProductService inner, FlowTracer tracer) : IProductService
{
    public Task<List<Product>> GetAllAsync()
    {
        return tracer.TraceAsync(
            "ProductsController", "GetAll",
            "ProductService", "GetAllAsync",
            "Service",
            () => inner.GetAllAsync(),
            payloadType: "Product[]");
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "ProductsController", "GetById",
            "ProductService", "GetByIdAsync",
            "Service",
            () => inner.GetByIdAsync(id),
            input: new { id }, payloadType: "Product");
    }

    public Task AddAsync(ProductDetails productDetails)
    {
        return tracer.TraceAsync(
            "ProductsController", "Add",
            "ProductService", "AddAsync",
            "Service",
            () => inner.AddAsync(productDetails),
            input: productDetails, payloadType: "ProductDetails");
    }

    public Task UpdateAsync(Guid id, ProductDetails productDetails)
    {
        return tracer.TraceAsync(
            "ProductsController", "Update",
            "ProductService", "UpdateAsync",
            "Service",
            () => inner.UpdateAsync(id, productDetails),
            input: new { id, productDetails }, payloadType: "ProductDetails");
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "ProductsController", "Delete",
            "ProductService", "DeleteAsync",
            "Service",
            () => inner.DeleteAsync(id),
            input: new { id }, payloadType: "Guid");
    }
}
