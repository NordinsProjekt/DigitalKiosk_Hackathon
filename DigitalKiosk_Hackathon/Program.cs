
using EF_MSSQL;
using EF_MSSQL.Repositories;
using EF_MSSQL.Seeders;
using Services;
using Services.Interfaces;

namespace Api;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.WebHost.UseSentry(o =>
        //{
        //    o.Dsn = builder.Configuration["Sentry:Dsn"];
        //    // Enable Sentry internal debug logging only in Development
        //    o.Debug = builder.Environment.IsDevelopment();

        //    // Configure traces sample rate from configuration if available,
        //    // otherwise use 100% in Development and a lower rate in other environments.
        //    var tracesSampleRateConfig = builder.Configuration["Sentry:TracesSampleRate"];
        //    if (double.TryParse(tracesSampleRateConfig, out var tracesSampleRate))
        //    {
        //        o.TracesSampleRate = tracesSampleRate;
        //    }
        //    else
        //    {
        //        o.TracesSampleRate = builder.Environment.IsDevelopment() ? 1.0 : 0.1;
        //    }
        //    // Enable logs to be sent to Sentry
        //    o.EnableLogs = true;
        //});

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<KioskDbContext>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IDiscountedProductRepository, DiscountedProductRepository>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IDiscountedProductService, DiscountedProductService>();

        var app = builder.Build();

        app.UseCors("AllowAll"); //Unsafe only for debugging

        // Configure the HTTP request pipeline.
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Kiosk API v1");
            options.RoutePrefix = string.Empty;
        });

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<KioskDbContext>();

        if (!db.Products.Any())
        {
            var products = ProductSeeder.Generate(3000);
            db.Products.AddRange(products);
            await db.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {products.Count} products.");
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
