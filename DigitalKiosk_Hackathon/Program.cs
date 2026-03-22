
using Services;
using Services.Interfaces;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseSentry(o =>
        {
            o.Dsn = builder.Configuration["Sentry:Dsn"];
            // Enable Sentry internal debug logging only in Development
            o.Debug = builder.Environment.IsDevelopment();

            // Configure traces sample rate from configuration if available,
            // otherwise use 100% in Development and a lower rate in other environments.
            var tracesSampleRateConfig = builder.Configuration["Sentry:TracesSampleRate"];
            if (double.TryParse(tracesSampleRateConfig, out var tracesSampleRate))
            {
                o.TracesSampleRate = tracesSampleRate;
            }
            else
            {
                o.TracesSampleRate = builder.Environment.IsDevelopment() ? 1.0 : 0.1;
            }
            // Enable logs to be sent to Sentry
            o.EnableLogs = true;
        });

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IProductService, ProductService>();

        var app = builder.Build();

        app.UseCors("AllowAll"); //Unsafe only for debugging

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Kiosk API v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
