using Entities;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingProductRepository(IProductRepository inner, FlowTracer tracer) : IProductRepository
{
    public Task<List<Product>> GetAllAsync()
    {
        return tracer.TraceAsync(
            "ProductService", "GetAllAsync",
            "ProductRepository", "GetAllAsync",
            "Repository",
            () => inner.GetAllAsync());
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "ProductService", "GetByIdAsync",
            "ProductRepository", "GetByIdAsync",
            "Repository",
            () => inner.GetByIdAsync(id));
    }

    public Task AddAsync(Product product)
    {
        return tracer.TraceAsync(
            "ProductService", "AddAsync",
            "ProductRepository", "AddAsync",
            "Repository",
            () => inner.AddAsync(product));
    }

    public Task UpdateAsync(Product product)
    {
        return tracer.TraceAsync(
            "ProductService", "UpdateAsync",
            "ProductRepository", "UpdateAsync",
            "Repository",
            () => inner.UpdateAsync(product));
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "ProductService", "DeleteAsync",
            "ProductRepository", "DeleteAsync",
            "Repository",
            () => inner.DeleteAsync(id));
    }

    public Task<List<Product>> FilterAsync(string query)
    {
        return tracer.TraceAsync(
            "ProductService", "FilterAsync",
            "ProductRepository", "FilterAsync",
            "Repository",
            () => inner.FilterAsync(query));
    }
}
