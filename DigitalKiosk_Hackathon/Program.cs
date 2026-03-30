
using EF_MSSQL;
using EF_MSSQL.Repositories;
using EF_MSSQL.Seeders;
using FlowVisualizer.Core;
using FlowVisualizer.Core.Adapters;
using FlowVisualizer.Core.Decorators;
using FlowVisualizer.Core.Hub;
using FlowVisualizer.Core.Interceptors;
using FlowVisualizer.Core.Middleware;
using Services;
using Services.Interfaces;

namespace Api;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();

        // --- FlowVisualizer: SignalR + tracing infrastructure ---
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IFlowEventSink, SignalRFlowEventSink>();
        builder.Services.AddScoped<FlowTracer>();
        builder.Services.AddSingleton<FlowDbCommandInterceptor>();

        // DbContext with tracing interceptor
        builder.Services.AddDbContext<KioskDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<FlowDbCommandInterceptor>();
            options.AddInterceptors(interceptor);
        });

        // Factory: IProductFactory → TracingProductFactory → ProductFactoryAdapter
        builder.Services.AddScoped<IProductFactory>(sp =>
            new TracingProductFactory(
                new ProductFactoryAdapter(),
                sp.GetRequiredService<FlowTracer>()));

        // Repositories: Interface → TracingDecorator → ConcreteRepository
        builder.Services.AddScoped<ProductRepository>();
        builder.Services.AddScoped<IProductRepository>(sp =>
            new TracingProductRepository(
                sp.GetRequiredService<ProductRepository>(),
                sp.GetRequiredService<FlowTracer>()));

        builder.Services.AddScoped<CustomerRepository>();
        builder.Services.AddScoped<ICustomerRepository>(sp =>
            new TracingCustomerRepository(
                sp.GetRequiredService<CustomerRepository>(),
                sp.GetRequiredService<FlowTracer>()));

        builder.Services.AddScoped<DiscountedProductRepository>();
        builder.Services.AddScoped<IDiscountedProductRepository>(sp =>
            new TracingDiscountedProductRepository(
                sp.GetRequiredService<DiscountedProductRepository>(),
                sp.GetRequiredService<FlowTracer>()));

        // Services: Interface → TracingDecorator → ConcreteService
        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<IProductService>(sp =>
            new TracingProductService(
                sp.GetRequiredService<ProductService>(),
                sp.GetRequiredService<FlowTracer>()));

        builder.Services.AddScoped<CustomerService>();
        builder.Services.AddScoped<ICustomerService>(sp =>
            new TracingCustomerService(
                sp.GetRequiredService<CustomerService>(),
                sp.GetRequiredService<FlowTracer>()));

        builder.Services.AddScoped<DiscountedProductService>();
        builder.Services.AddScoped<IDiscountedProductService>(sp =>
            new TracingDiscountedProductService(
                sp.GetRequiredService<DiscountedProductService>(),
                sp.GetRequiredService<FlowTracer>()));

        var app = builder.Build();

        // Correlation middleware must be first
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseCors("AllowAll"); //Unsafe only for debugging

        // Configure the HTTP request pipeline.
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Kiosk API v1");
            options.RoutePrefix = string.Empty;
        });

        // FlowVisualizer SignalR hub
        app.MapHub<FlowHub>("/flow-hub");

        // Serve flow dashboard static files
        app.UseStaticFiles();

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
