using Microsoft.AspNetCore.Http;

namespace FlowVisualizer.Core.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        FlowCorrelation.Current = Guid.NewGuid();
        context.Response.Headers["X-Correlation-Id"] = FlowCorrelation.Current.ToString();
        await next(context);
    }
}
