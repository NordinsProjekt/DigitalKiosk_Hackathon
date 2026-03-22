
namespace DigitalKiosk_Hackathon;

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
        SentrySdk.CaptureMessage("Hello Sentry"); //Test

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
