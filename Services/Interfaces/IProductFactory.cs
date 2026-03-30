using Entities;
using Factories.Models;

namespace Services.Interfaces;

public interface IProductFactory
{
    Product Create(ProductDetails productDetails);
}
