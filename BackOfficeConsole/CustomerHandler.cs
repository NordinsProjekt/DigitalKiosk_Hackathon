using BackOfficeConsole.Validation;
using Entities;
using Services.Interfaces;

namespace BackOfficeConsole;

public class CustomerHandler
{
    private ICustomerService _customerService;

    public CustomerHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task AddCustomer()
    {
        if (!CustomerValidation.TryGetName(out var name)) return;
        if (!CustomerValidation.TryGetPersonalIdentityNumber(out var personalIdentity)) return;

        var customer = new Customer()
        {
            Name = name,
            PersonalIdentityNumber = personalIdentity
        };

        await _customerService.AddAsync(customer);
        Console.WriteLine("Kund tillagd!");
    }

    public async Task ListCustomer()
    {
        Console.WriteLine("Laddar...");
        var customers = await _customerService.GetAllAsync();
        Console.Clear();
        foreach (var c in customers.OrderBy(c => c.Name))
        {
            Console.WriteLine($"{c.Name} - {c.PersonalIdentityNumber}");
        }
    }
}