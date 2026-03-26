using System;
using System.Collections.Generic;
using System.Text;

using Entities.Enums;
using Factories;

namespace Domain.FactoriesTests
{
    public class ProductFactoryTests
    {
        [Fact]
        public void Create_ValidDetails_ShouldReturnProduct()
        {
            // Arrange
            var details = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            );

            // Act
            var product = ProductFactory.Create(details);

            // Assert
            Assert.Equal(details.Name, product.Name);
            Assert.Equal(details.Description, product.Description);
            Assert.Equal(details.ShelfLocation, product.ShelfLocation);
            Assert.Equal(details.Section, product.Section);
            Assert.Equal(details.Price, product.Price);
            Assert.NotEqual(Guid.Empty, product.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_InvalidName_ShouldThrow(string invalidName)
        {
            // Arrange
            var details = new ProductDetails(
                name: invalidName,
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProductFactory.Create(details));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_InvalidDescription_ShouldThrow(string invalidDescription)
        {
            // Arrange
            var details = new ProductDetails(
                name: "Product name",
                description: invalidDescription,
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProductFactory.Create(details));
        }

        [Fact]
        public void Create_InvalidShelfLocation_ShouldThrow()
        {
            // Example if you want to test invalid enum values
            var details = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: (ShelfLocation)999,
                section: Section.Baby,
                price: 10m
            );

            Assert.Throws<ArgumentException>(() => ProductFactory.Create(details));
        }

        [Fact]
        public void Create_InvalidSection_ShouldThrow()
        {
            var details = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: (Section)999,
                price: 10m
            );

            Assert.Throws<ArgumentException>(() => ProductFactory.Create(details));
        }

        [Fact]
        public void Create_NegativePrice_ShouldThrow()
        {
            // Arrange
            var details = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: -10m
            );

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ProductFactory.Create(details));
        }

        [Fact]
        public void Create_ZeroPrice_ShouldWork()
        {
            // Arrange
            var details = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 0m
            );

            // Act
            var product = ProductFactory.Create(details);

            // Assert
            Assert.Equal(0m, product.Price);
        }

        [Fact]
        public void Create_ShouldInitializeEmptyDiscountedProducts()
        {
            // Arrange
            var details = new ProductDetails(
                "Product name",
                "Product description",
                ShelfLocation.A1,
                Section.Baby,
                10m
            );

            // Act
            var product = ProductFactory.Create(details);

            // Assert
            Assert.Empty(product.DiscountedProducts);
        }
    }
}
