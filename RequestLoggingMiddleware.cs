using System.Diagnostics;

namespace Library_Management.Middleware;

/// <summary>
/// TASK 2.4 — Logs HTTP method, path, response status code, and elapsed time
/// (ms) for every request that passes through the pipeline.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path;

        try
        {
            // Pass control to the rest of the pipeline (controllers, etc.)
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "{Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                method,
                path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

/// <summary>
/// Extension method for clean registration in Program.cs:
/// app.UseMiddleware&lt;RequestLoggingMiddleware&gt;() also works,
/// but app.UseRequestLogging() reads better.
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
