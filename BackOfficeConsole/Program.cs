namespace BackOfficeConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseSentry(o =>
        {
            o.Dsn = builder.Configuration["Sentry:Dsn"];
            // When configuring for the first time, to see what the SDK is doing:
            o.Debug = true;
            // Set TracesSampleRate to 1.0 to capture 100%
            // of transactions for tracing.
            // We recommend adjusting this value in production
            o.TracesSampleRate = 1.0;
            // Enable logs to be sent to Sentry
            o.EnableLogs = true;
        });

        var app = builder.Build();

        SentrySdk.CaptureMessage("Hello Sentry"); //Test

        await app.RunAsync();
    }
}
