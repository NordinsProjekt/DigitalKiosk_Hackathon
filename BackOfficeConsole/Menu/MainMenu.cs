namespace BackOfficeConsole.Menu;

public class MainMenu : BaseMenu
{
    private readonly ProductMenu _productMenu;
    private readonly CustomerMenu _customerMenu;

    public MainMenu(ProductHandler productHandler, CustomerHandler customerHandler)
    {
        _productMenu = new ProductMenu(productHandler);
        _customerMenu = new CustomerMenu(customerHandler);
    }

    protected override string Title => "BackOffice";

    protected override string[] Options => new[]
    {
        "Produkter",
        "Kunder",
        "Avsluta"
    };

    protected override async Task<bool> ExecuteChoiceAsync(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0: await _productMenu.Run(); return true;
            case 1: await _customerMenu.Run(); return true;
            case 2: Environment.Exit(0); return false;
            default: return false;
        }
    }
}