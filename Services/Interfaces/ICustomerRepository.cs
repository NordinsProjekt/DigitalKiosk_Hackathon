using Entities;

namespace Services.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task AddAsync(Customer customer);
    Task UpdateNameAsync(Customer customer);
    Task UpdateIdentityNumberAsync(Customer customer);
    Task DeleteAsync(Guid id);
}