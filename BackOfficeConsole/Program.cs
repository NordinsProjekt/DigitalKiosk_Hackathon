using Microsoft.Extensions.Configuration;

namespace BackOfficeConsole;

internal class Program
{
    static void Main(string[] args)
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
    }
}
