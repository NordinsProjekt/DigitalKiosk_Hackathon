using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using NSubstitute;
using Services;
using Services.Interfaces;

namespace Application.ServicesTests
{
    public class CustomerTests
    {
        private readonly ICustomerService _service;
        private readonly ICustomerRepository _customerRepo;

        public CustomerTests()
        {
            _customerRepo = Substitute.For<ICustomerRepository>();
            _service = new CustomerService(_customerRepo);
        }

        [Fact]
        public async Task UpdateNameAsync_ShouldCallRepoOnce()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "NewName" };

            await _service.UpdateNameAsync(customer);

            await _customerRepo.Received(1).UpdateNameAsync(customer);
        }

        [Fact]
        public async Task UpdateIdentityNumberAsync_ShouldCallRepoOnce()
        {
            var customer = new Customer { Id = Guid.NewGuid(), PersonalIdentityNumber = "111111-1111" };

            await _service.UpdateIdentityNumberAsync(customer);

            await _customerRepo.Received(1).UpdateIdentityNumberAsync(customer);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepoOnce()
        {
            var id = Guid.NewGuid();

            await _service.DeleteAsync(id);

            await _customerRepo.Received(1).DeleteAsync(id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            var customers = new List<Customer> { new Customer(), new Customer() };
            _customerRepo.GetAllAsync().Returns(customers);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCustomer()
        {
            var id = Guid.NewGuid();
            var customer = new Customer { Id = id };
            _customerRepo.GetByIdAsync(id).Returns(customer);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(id, result?.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _customerRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Customer?)null);

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepoOnce()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test" };

            await _service.AddAsync(customer);

            await _customerRepo.Received(1).AddAsync(customer);
        }
    }
}
