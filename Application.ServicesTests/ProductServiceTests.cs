using System;
using System.Collections.Generic;
using System.Text;

using Services;
using Services.Interfaces;
using Entities;
using NSubstitute;

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
            var products = new List<Product>
            {
                new Product(),
                new Product()
            };
            _repository.GetAllAsync().Returns(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
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
            var product = new Product { Id = existingId };
            _repository.GetByIdAsync(existingId).Returns(product);

            // Act
            var result = await _service.GetByIdAsync(existingId);

            // Assert
            Assert.Equal(existingId, result.Id);
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
            var product = new Product();
            _repository.AddAsync(product).Returns(Task.CompletedTask);

            // Act
            await _service.AddAsync(product);

            // Assert
            await _repository.Received(1).AddAsync(product);
        }

        [Fact]
        public async Task ProductService_UpdateAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var product = new Product();
            _repository.UpdateAsync(product).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(product);

            // Assert
            await _repository.Received(1).UpdateAsync(product);
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
