using BackOfficeConsole.Menu;
using EF_MSSQL;
using EF_MSSQL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;

namespace BackOfficeConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase);

        double tracesSampleRate;
        var configuredSampleRate = configuration["Sentry:TracesSampleRate"];
        if (!double.TryParse(
                configuredSampleRate,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out tracesSampleRate))
        {
            // Default to full tracing in Development, reduced sampling elsewhere
            tracesSampleRate = isDevelopment ? 1.0 : 0.2;
        }

        using var _ = SentrySdk.Init(o =>
        {
            o.Dsn = configuration["Sentry:Dsn"];
            // Enable Sentry SDK debug logging only in Development
            o.Debug = isDevelopment;
            o.TracesSampleRate = tracesSampleRate;
            o.EnableLogs = true;
        });

        
        var services = new ServiceCollection();

        services.AddDbContext<KioskDbContext>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
        var productHandler = new ProductHandler(productService);
        var customerHandler = new CustomerHandler(customerService);

        var menu = new MenuData(new List<MenuOption>{
            new MenuOption("Lista produkter", async () => await productHandler.ListProducts()),
            new MenuOption("Lägg till produkt", async () => await productHandler.AddProduct()),
            new MenuOption("Redigera produkt", async () => await productHandler.EditProductAsync()),
            new MenuOption("Lista Kunder", async () => await customerHandler.ListCustomer()),
            new MenuOption("Lägg till kund", async () => await customerHandler.AddCustomer()),
            new MenuOption("Avsluta", () => { Environment.Exit(0); return Task.CompletedTask;})
             });

        await menu.Run();
    }
}

