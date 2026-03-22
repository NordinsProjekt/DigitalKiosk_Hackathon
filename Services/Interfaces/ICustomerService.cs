using Entities;

namespace Services.Interfaces;

public interface ICustomerService
{
    public Task<IEnumerable<Customer>> GetAllAsync();
    public Task<Customer?> GetByIdAsync(Guid id);
    public Task AddNameAsync(Customer customer);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(Customer customer);
}