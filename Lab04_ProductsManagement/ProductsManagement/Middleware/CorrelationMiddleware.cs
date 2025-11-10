using System.Diagnostics;

namespace ProductsManagement;
public class CorrelationMiddleware
{
    private const string CorrelationHeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationHeaderName, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString("D"); 
        }

        context.TraceIdentifier = correlationId.ToString();
        context.Response.Headers.TryAdd(CorrelationHeaderName, correlationId);

        await _next(context);
    }
}

public static class CorrelationMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationMiddleware>();
    }
}