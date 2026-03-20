namespace MVC_Di.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startAt = DateTime.UtcNow;
        await next(context);
        var duration = DateTime.UtcNow - startAt;

        logger.LogInformation(
            "Request {Method} {Path} finished with {StatusCode} in {ElapsedMs} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    }
}
