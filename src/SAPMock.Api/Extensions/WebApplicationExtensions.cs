using SAPMock.Configuration;
using SAPMock.Core;
using Microsoft.AspNetCore.Http;

namespace SAPMock.Api.Extensions;

/// <summary>
/// Extension methods for WebApplication to register dynamic endpoints.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Registers dynamic endpoints based on SAP system configuration.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static async Task<WebApplication> RegisterSAPEndpoints(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();
        var systemRegistry = app.Services.GetRequiredService<ISAPSystemRegistry>();

        logger.LogInformation("Starting dynamic SAP endpoint registration...");

        try
        {
            // Get all registered systems
            var systems = await systemRegistry.GetAllSystemsAsync();
            int totalEndpoints = 0;

            foreach (var system in systems)
            {
                // Get modules for each system
                var modules = await systemRegistry.GetModulesForSystem(system.SystemId);
                
                foreach (var module in modules)
                {
                    // Register endpoints for each module
                    RegisterModuleEndpoints(app, system, module, logger);
                    totalEndpoints += module.Endpoints.Count();
                }
            }

            logger.LogInformation("Successfully registered {TotalEndpoints} SAP endpoints from {SystemCount} systems", 
                totalEndpoints, systems.Count());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during SAP endpoint registration");
            throw;
        }

        return app;
    }

    /// <summary>
    /// Registers endpoints for a specific module.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <param name="system">The SAP system.</param>
    /// <param name="module">The SAP module.</param>
    /// <param name="logger">The logger instance.</param>
    private static void RegisterModuleEndpoints(WebApplication app, ISAPSystem system, ISAPModule module, ILogger logger)
    {
        logger.LogDebug("Registering endpoints for system {SystemId}, module {ModuleId}", 
            system.SystemId, module.ModuleId);

        foreach (var endpoint in module.Endpoints)
        {
            try
            {
                RegisterEndpoint(app, system, module, endpoint, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to register endpoint {Method} {Path} for system {SystemId}, module {ModuleId}", 
                    endpoint.Method, endpoint.Path, system.SystemId, module.ModuleId);
            }
        }
    }

    /// <summary>
    /// Registers a single endpoint with ASP.NET Core routing.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <param name="system">The SAP system.</param>
    /// <param name="module">The SAP module.</param>
    /// <param name="endpoint">The SAP endpoint.</param>
    /// <param name="logger">The logger instance.</param>
    private static void RegisterEndpoint(WebApplication app, ISAPSystem system, ISAPModule module, ISAPEndpoint endpoint, ILogger logger)
    {
        // Build the route pattern using convention: /api/{systemId}/{moduleId}{endpointPath}
        var routePattern = $"/api/{system.SystemId}/{module.ModuleId}{endpoint.Path}";
        
        // Create endpoint display name for logging and OpenAPI
        var endpointName = $"{system.SystemId}_{module.ModuleId}_{endpoint.Method}_{endpoint.Path.Replace("/", "_").Replace("{", "").Replace("}", "")}";

        logger.LogDebug("Registering endpoint: {Method} {RoutePattern}", endpoint.Method, routePattern);

        // Register the endpoint based on HTTP method
        switch (endpoint.Method.ToUpperInvariant())
        {
            case "GET":
                app.MapGet(routePattern, async (HttpContext context) =>
                {
                    return await HandleRequest(context, endpoint, system, module, logger);
                })
                .WithName(endpointName)
                .WithOpenApi();
                break;

            case "POST":
                app.MapPost(routePattern, async (HttpContext context) =>
                {
                    return await HandleRequest(context, endpoint, system, module, logger);
                })
                .WithName(endpointName)
                .WithOpenApi();
                break;

            case "PUT":
                app.MapPut(routePattern, async (HttpContext context) =>
                {
                    return await HandleRequest(context, endpoint, system, module, logger);
                })
                .WithName(endpointName)
                .WithOpenApi();
                break;

            case "DELETE":
                app.MapDelete(routePattern, async (HttpContext context) =>
                {
                    return await HandleRequest(context, endpoint, system, module, logger);
                })
                .WithName(endpointName)
                .WithOpenApi();
                break;

            default:
                logger.LogWarning("Unsupported HTTP method {Method} for endpoint {Path}", 
                    endpoint.Method, endpoint.Path);
                break;
        }

        logger.LogInformation("Registered endpoint: {Method} {RoutePattern} -> {EndpointName}", 
            endpoint.Method, routePattern, endpointName);
    }

    /// <summary>
    /// Handles HTTP requests for dynamically registered endpoints.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="endpoint">The SAP endpoint configuration.</param>
    /// <param name="system">The SAP system.</param>
    /// <param name="module">The SAP module.</param>
    /// <param name="logger">The logger instance.</param>
    /// <returns>The HTTP response.</returns>
    private static async Task<IResult> HandleRequest(HttpContext context, ISAPEndpoint endpoint, ISAPSystem system, ISAPModule module, ILogger logger)
    {
        try
        {
            logger.LogDebug("Handling request for {Method} {Path} on system {SystemId}, module {ModuleId}", 
                endpoint.Method, endpoint.Path, system.SystemId, module.ModuleId);

            // Check for error simulation
            var errorSimulationService = context.RequestServices.GetService<IErrorSimulationService>();
            if (errorSimulationService != null)
            {
                // Convert IHeaderDictionary to Dictionary<string, string>
                var headers = context.Request.Headers.ToDictionary(
                    h => h.Key, 
                    h => h.Value.FirstOrDefault() ?? string.Empty);

                var errorConfig = await errorSimulationService.ShouldSimulateErrorAsync(
                    system.SystemId, module.ModuleId, endpoint.Path, headers);

                if (errorConfig != null)
                {
                    // Handle timeout simulation
                    if (errorConfig.ErrorType == ErrorType.Timeout && errorConfig.DelayMs > 0)
                    {
                        await Task.Delay(errorConfig.DelayMs);
                    }

                    // Create error response
                    var errorResponse = errorSimulationService.CreateErrorResponse(errorConfig);
                    
                    // Log the simulated error
                    await errorSimulationService.LogSimulatedErrorAsync(
                        system.SystemId, module.ModuleId, endpoint.Path, errorConfig, errorResponse);

                    // Return appropriate HTTP status based on error type
                    var statusCode = errorConfig.ErrorType switch
                    {
                        ErrorType.Timeout => StatusCodes.Status408RequestTimeout,
                        ErrorType.Authorization => StatusCodes.Status401Unauthorized,
                        ErrorType.Business => StatusCodes.Status400BadRequest,
                        ErrorType.System => StatusCodes.Status500InternalServerError,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    return Results.Json(errorResponse, statusCode: statusCode);
                }
            }

            // Extract request body for POST/PUT requests
            object? requestData = null;
            if (context.Request.Method.ToUpperInvariant() == "POST" || context.Request.Method.ToUpperInvariant() == "PUT")
            {
                if (context.Request.HasJsonContentType())
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(requestBody))
                    {
                        requestData = System.Text.Json.JsonSerializer.Deserialize<object>(requestBody);
                    }
                }
            }

            // Extract route parameters
            var routeParameters = new Dictionary<string, object?>();
            foreach (var routeValue in context.Request.RouteValues)
            {
                routeParameters[routeValue.Key] = routeValue.Value;
            }

            // Create a combined request object with body and route parameters
            var combinedRequest = new
            {
                Body = requestData,
                RouteParameters = routeParameters,
                QueryParameters = context.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString())
            };

            // Call the endpoint handler
            var response = await endpoint.Handler(combinedRequest);

            // Return the response
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling request for {Method} {Path} on system {SystemId}, module {ModuleId}", 
                endpoint.Method, endpoint.Path, system.SystemId, module.ModuleId);
            
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error");
        }
    }
}