using Entities;
using Factories.Models;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingProductFactory(IProductFactory inner, FlowTracer tracer) : IProductFactory
{
    public Product Create(ProductDetails productDetails)
    {
        return tracer.TraceSync(
            "ProductService", "AddAsync",
            "ProductFactory", "Create",
            "Factory",
            () => inner.Create(productDetails),
            input: productDetails, payloadType: "Product");
    }
}
