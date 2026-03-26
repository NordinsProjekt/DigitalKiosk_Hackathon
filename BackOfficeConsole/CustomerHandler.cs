using BackOfficeConsole.Validation;
using Entities;
using Services.Interfaces;

namespace BackOfficeConsole;

public class CustomerHandler
{
    private readonly ICustomerService _customerService;

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
        var customers = await _customerService.GetAllAsync();
        foreach (var c in customers)
        {
            Console.WriteLine($"{c.Name} - {c.PersonalIdentityNumber}");
        }
    }
    public async Task EditCustomerAsync()
    {
        var customer = await SelectCustomerAsync();
        if (customer == null) return;
        await HandleCustomerMenuAsync(customer);
    }

    private async Task<Customer?> SelectCustomerAsync()
    {
        var customers = await _customerService.GetAllAsync();
        if (!customers.Any())
        {
            Console.WriteLine("Inga kunder finns i systemet.");
            return null;
        }

        Console.WriteLine("\n=== Välj kund att redigera ===");
        for (int i = 0; i < customers.Count; i++)
            Console.WriteLine($"{i + 1}. {customers[i].Name} | {customers[i].PersonalIdentityNumber}");

        Console.Write("\nAnge nummer: ");
        if (!int.TryParse(Console.ReadLine(), out var choice)
            || choice < 1 || choice > customers.Count)
        {
            Console.WriteLine("Ogiltigt val!");
            return null;
        }

        return customers[choice - 1];
    }

    private async Task HandleCustomerMenuAsync(Customer customer)
    {
        Console.WriteLine($"\nRedigerar: {customer.Name}");
        Console.WriteLine("1. Ändra namn");
        Console.WriteLine("2. Ändra personnummer");
        Console.WriteLine("3. Avbryt");
        Console.Write("\nGör ett val: ");

        if (!int.TryParse(Console.ReadLine(), out int menuChoice))
        {
            Console.WriteLine("Ogiltigt val!");
            return;
        }

        switch (menuChoice)
        {
            case 1:
                if (!CustomerValidation.TryGetName(out var name)) return;
                customer.Name = name;
                await _customerService.UpdateNameAsync(customer);
                break;

            case 2:
                if (!CustomerValidation.TryGetPersonalIdentityNumber(out var personalIdentity)) return;
                customer.PersonalIdentityNumber = personalIdentity;
                await _customerService.UpdateIdentityNumberAsync(customer);
                break;

            case 3:
                Console.WriteLine("Avbrutet.");
                return;

            default:
                Console.WriteLine("Ogiltigt val.");
                return;
        }

        Console.WriteLine("✓ Kunden har uppdaterats!");
    }
}