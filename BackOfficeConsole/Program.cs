namespace BackOfficeConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseSentry(o =>
        {
            o.Dsn = builder.Configuration["Sentry:Dsn"];

            var environmentName = global::System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = string.Equals(environmentName, "Development", global::System.StringComparison.OrdinalIgnoreCase);

            // Enable Sentry SDK debug logging only in Development
            o.Debug = isDevelopment;

            // Allow TracesSampleRate to be configured; fall back to safer defaults per environment
            double tracesSampleRate;
            var configuredSampleRate = builder.Configuration["Sentry:TracesSampleRate"];
            if (!global::System.Double.TryParse(
                    configuredSampleRate,
                    global::System.Globalization.NumberStyles.Float,
                    global::System.Globalization.CultureInfo.InvariantCulture,
                    out tracesSampleRate))
            {
                // Default to full tracing in Development, reduced sampling elsewhere
                tracesSampleRate = isDevelopment ? 1.0 : 0.2;
            }

            o.TracesSampleRate = tracesSampleRate;
            // Enable logs to be sent to Sentry
            o.EnableLogs = true;
        });

        var app = builder.Build();

#if DEBUG
        // Test message to verify Sentry integration in debug builds only.
        SentrySdk.CaptureMessage("Hello Sentry"); //Test
#endif

        await app.RunAsync();
    }
}
