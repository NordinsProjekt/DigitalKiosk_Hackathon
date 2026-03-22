using Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services;

public class CustomerService : ICustomerService
{
    public Task AddAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task <List<Customer>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}
