namespace BackOfficeConsole.Menu;

public class CustomerMenu : BaseMenu
{
    private readonly CustomerHandler _customerHandler;

    public CustomerMenu(CustomerHandler customerHandler)
    {
        _customerHandler = customerHandler;
    }

    protected override string Title => "Kunder";

    protected override string[] Options => new[]
    {
        "Lista kunder",
        "Lägg till kund",
        "Redigera kund",
        "Tillbaka"
    };

    protected override async Task<bool> ExecuteChoiceAsync(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0: await _customerHandler.ListCustomer(); return true;
            case 1: await _customerHandler.AddCustomer(); return true;
            case 2: await _customerHandler.EditCustomerAsync(); return true;
            case 3: return false;
            default: return false;
        }
    }
}