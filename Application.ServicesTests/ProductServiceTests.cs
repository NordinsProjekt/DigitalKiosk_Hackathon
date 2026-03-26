using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Entities;
using Entities.Enums;
using Factories.Models;
using Services;
using Services.Interfaces;

namespace Application.ServicesTests
{
    public class ProductServiceTests
    {
        private readonly ProductService _service;
        private readonly IProductRepository _repository;

        public ProductServiceTests()
        {
            _repository = Substitute.For<IProductRepository>();
            _service = new ProductService(_repository);
        }

        [Fact]
        public async Task ProductService_GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var product1 = new Product(
                name: "Product 1",
                description: "Description 1",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            )
            {
                Id = Guid.NewGuid()
            };

            var product2 = new Product(
                name: "Product 2",
                description: "Description 2",
                shelfLocation: ShelfLocation.B2,
                section: Section.Beverages,
                price: 20m
            )
            {
                Id = Guid.NewGuid()
            };

            var products = new List<Product> { product1, product2 };
            _repository.GetAllAsync().Returns(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Id == product1.Id);
            Assert.Contains(result, p => p.Id == product2.Id);
        }

        [Fact]
        public async Task ProductService_GetAllAsync_NoProducts_ShouldReturnEmptyList()
        {
            // Arrange
            _repository.GetAllAsync().Returns(new List<Product>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ProductService_GetByIdAsync_ProductExists_ShouldReturnProduct()
        {
            // Arrange
            var existingId = Guid.NewGuid();

            // Existing product in repository (realistic instance using constructor)
            var product = new Product(
                name: "Existing product",
                description: "Some description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            )
            {
                Id = existingId // preserve the original Id
            };

            _repository.GetByIdAsync(existingId).Returns(product);

            // Act
            var result = await _service.GetByIdAsync(existingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingId, result.Id);
            Assert.Equal("Existing product", result.Name);
            Assert.Equal("Some description", result.Description);
            Assert.Equal(ShelfLocation.A1, result.ShelfLocation);
            Assert.Equal(Section.Baby, result.Section);
            Assert.Equal(10m, result.Price);
        }


        [Fact]
        public async Task ProductService_GetByIdAsync_ProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            _repository.GetByIdAsync(nonExistingId).Returns((Product?)null);

            // Act
            var result = await _service.GetByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ProductService_AddAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var productDetails = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            );

            // Act
            await _service.AddAsync(productDetails);

            // Assert
            await _repository.Received(1).AddAsync(Arg.Any<Product>());
        }

        [Fact]
        public async Task ProductService_UpdateAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existing = new Product(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 10m
            )
            {
                Id = id
            };

            _repository.GetByIdAsync(id).Returns(existing);

            // New details for update
            var details = new ProductDetails(
                name: "Product name",
                description: "Product description",
                shelfLocation: ShelfLocation.A1,
                section: Section.Baby,
                price: 15m
            );

            // Act
            await _service.UpdateAsync(id, details);

            // Assert
            await _repository.Received(1).UpdateAsync(
                Arg.Is<Product>(p =>
                    p.Id == id &&
                    p.Name == details.Name &&
                    p.Description == details.Description &&
                    p.ShelfLocation == details.ShelfLocation &&
                    p.Section == details.Section &&
                    p.Price == details.Price
                )
            );
        }

        [Fact]
        public async Task ProductService_DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _repository.DeleteAsync(productId).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(productId);

            // Assert
            await _repository.Received(1).DeleteAsync(productId);
        }
    }
}
