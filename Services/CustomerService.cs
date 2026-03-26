using Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services;

public class CustomerService(ICustomerRepository repo) : ICustomerService
{
    public async Task <List<Customer>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await repo.GetByIdAsync(id);
    }

    public async Task AddAsync(Customer customer)
    {
        await repo.AddAsync(customer);
    }

    public async Task UpdateNameAsync(Customer customer)
    {
        await repo.UpdateNameAsync(customer);
    }

    public async Task UpdateIdentityNumberAsync(Customer customer)
    {
        await repo.UpdateIdentityNumberAsync(customer);
    }

    public async Task DeleteAsync(Guid id)
    {
        await repo.DeleteAsync(id);
    }
    public async Task<Customer?> GetByPersonalIdentityNumberAsync(string personalIdentityNumber)
    {
        return await repo.GetByPersonalIdentityNumberAsync(personalIdentityNumber);
    }
}
