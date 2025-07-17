using SAPMock.Api.Models;
using SAPMock.Api.Services;
using System.Diagnostics;
using System.Text;

namespace SAPMock.Api.Middleware;

/// <summary>
/// Middleware to log all HTTP requests for monitoring purposes.
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
        // Skip logging for Blazor hub connections and static files
        if (context.Request.Path.StartsWithSegments("/_framework") ||
            context.Request.Path.StartsWithSegments("/css") ||
            context.Request.Path.StartsWithSegments("/js") ||
            context.Request.Path.StartsWithSegments("/lib") ||
            context.Request.Path.StartsWithSegments("/_vs") ||
            context.Request.Path.StartsWithSegments("/favicon.ico") ||
            context.WebSockets.IsWebSocketRequest)
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var requestBody = string.Empty;
        var responseBody = string.Empty;

        // Capture request body for non-GET requests
        if (context.Request.Method != "GET" && context.Request.ContentLength.HasValue && context.Request.ContentLength > 0)
        {
            try
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                requestBody = body;
                context.Request.Body.Position = 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read request body");
            }
        }

        // Capture response
        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await _next(context);

        stopwatch.Stop();

        // Read response body
        try
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read response body");
        }
        finally
        {
            context.Response.Body = originalResponseBodyStream;
        }

        // Parse system and module from path
        var pathSegments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var system = string.Empty;
        var module = string.Empty;

        if (pathSegments?.Length >= 3 && pathSegments[0] == "api")
        {
            system = pathSegments[1];
            module = pathSegments[2];
        }

        // Create log entry
        var logEntry = new RequestLogEntry
        {
            Timestamp = DateTime.UtcNow,
            Method = context.Request.Method,
            Path = context.Request.Path.Value ?? string.Empty,
            System = system,
            Module = module,
            StatusCode = context.Response.StatusCode,
            ResponseTimeMs = stopwatch.ElapsedMilliseconds,
            Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault() ?? string.Empty),
            RequestBody = requestBody,
            ResponseBody = responseBody,
            UserAgent = context.Request.Headers.UserAgent.FirstOrDefault() ?? string.Empty,
            RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty
        };

        // Log the request
        var requestMonitorService = context.RequestServices.GetService<IRequestMonitorService>();
        requestMonitorService?.LogRequest(logEntry);
    }
}