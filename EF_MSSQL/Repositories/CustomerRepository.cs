using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace EF_MSSQL.Repositories;

public class CustomerRepository(KioskDbContext context) : ICustomerRepository
{
    private readonly KioskDbContext _context = context;

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers.AsNoTracking().ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNameAsync(Customer customer)
    {
        var entry = _context.Entry(customer);
        entry.Property(c => c.Name).IsModified = true;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateIdentityNumberAsync(Customer customer)
    {
        var entry = _context.Entry(customer);
        entry.Property(c => c.PersonalIdentityNumber).IsModified = true;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}