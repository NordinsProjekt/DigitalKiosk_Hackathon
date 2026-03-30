using Entities;
using Factories;
using Factories.Models;
using Services.Interfaces;

namespace FlowVisualizer.Core.Adapters;

public class ProductFactoryAdapter : IProductFactory
{
    public Product Create(ProductDetails productDetails)
    {
        return ProductFactory.Create(productDetails);
    }
}
