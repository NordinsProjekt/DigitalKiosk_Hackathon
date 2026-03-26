using System;

using Entities;
using Entities.Enums;
using Factories.Models;

namespace Factories
{
    public static class ProductFactory
    {
        public static Product Create(ProductDetails productDetails)
        {
            if (string.IsNullOrWhiteSpace(productDetails.Name))
                throw new ArgumentException("Product name cannot be empty.", nameof(productDetails.Name));

            if (productDetails.Name.Length > 256)
                throw new ArgumentException("Product name cannot exceed 256 characters.", nameof(productDetails.Name));

            if (string.IsNullOrWhiteSpace(productDetails.Description))
                throw new ArgumentException("Product description cannot be empty.", nameof(productDetails.Description));

            if (productDetails.Description.Length > 1000)
                throw new ArgumentException("Product description cannot exceed 1000 characters.", nameof(productDetails.Description));

            if (!Enum.IsDefined(typeof(ShelfLocation), productDetails.ShelfLocation))
                throw new ArgumentException("Invalid ShelfLocation value.", nameof(productDetails.ShelfLocation));

            if (!Enum.IsDefined(typeof(Section), productDetails.Section))
                throw new ArgumentException("Invalid Section value.", nameof(productDetails.Section));

            if (productDetails.Price < 0)
                throw new ArgumentOutOfRangeException(nameof(productDetails.Price), "Price cannot be negative.");

            var product = new Product(
                productDetails.Name,
                productDetails.Description,
                productDetails.ShelfLocation,
                productDetails.Section,
                productDetails.Price
            );

            return product;
        }
    }
}
