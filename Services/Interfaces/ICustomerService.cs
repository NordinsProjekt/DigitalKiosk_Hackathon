using Entities;

namespace Services.Interfaces;

public interface ICustomerService
{
    public Task<List<Customer>> GetAllAsync();
    public Task<Customer?> GetByIdAsync(Guid id);
    public Task AddAsync(Customer customer);
    public Task DeleteAsync(Guid id);
    public Task UpdateNameAsync(Customer customer);
    public Task UpdateIdentityNumberAsync(Customer customer);
    public Task<Customer?> GetByPersonalIdentityNumberAsy
}