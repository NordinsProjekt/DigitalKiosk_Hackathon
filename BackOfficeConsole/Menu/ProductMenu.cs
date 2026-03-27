namespace BackOfficeConsole.Menu;

public class ProductMenu : BaseMenu
{
    private readonly ProductHandler _productHandler;

    public ProductMenu(ProductHandler productHandler)
    {
        _productHandler = productHandler;
    }

    protected override string Title => "Produkter";

    protected override string[] Options => new[]
    {
        "Lista produkter",
        "Lägg till produkt",
        "Redigera produkt",
        "Tillbaka"
    };

    protected override async Task<bool> ExecuteChoiceAsync(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0: await _productHandler.ListProducts(); return true;
            case 1: await _productHandler.AddProduct(); return true;
            case 2: await _productHandler.EditProductAsync(); return true;
            case 3: return false;
            default: return false;
        }
    }
}