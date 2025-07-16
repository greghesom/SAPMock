using SAPMock.Configuration;
using SAPMock.Core;

namespace SAPMock.Api.Services;

/// <summary>
/// Hosted service that dynamically registers API endpoints based on configuration.
/// </summary>
public class EndpointRegistrationService : IHostedService
{
    private readonly ISAPSystemRegistry _systemRegistry;
    private readonly ILogger<EndpointRegistrationService> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EndpointRegistrationService"/> class.
    /// </summary>
    /// <param name="systemRegistry">The system registry to load configurations from.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public EndpointRegistrationService(
        ISAPSystemRegistry systemRegistry,
        ILogger<EndpointRegistrationService> logger,
        IServiceProvider serviceProvider)
    {
        _systemRegistry = systemRegistry ?? throw new ArgumentNullException(nameof(systemRegistry));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Starts the service and logs endpoint registration information.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting dynamic endpoint registration service...");

        try
        {
            // Get all registered systems
            var systems = await _systemRegistry.GetAllSystemsAsync();
            int totalEndpoints = 0;

            foreach (var system in systems)
            {
                // Get modules for each system
                var modules = await _systemRegistry.GetModulesForSystem(system.SystemId);
                
                foreach (var module in modules)
                {
                    // Log endpoint information
                    LogModuleEndpoints(system, module);
                    totalEndpoints += module.Endpoints.Count();
                }
            }

            _logger.LogInformation("Dynamic endpoint registration service started. {TotalEndpoints} endpoints available from {SystemCount} systems", 
                totalEndpoints, systems.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during endpoint registration service startup");
            throw;
        }
    }

    /// <summary>
    /// Stops the service.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping dynamic endpoint registration service");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Logs endpoint information for a specific module.
    /// </summary>
    /// <param name="system">The SAP system.</param>
    /// <param name="module">The SAP module.</param>
    private void LogModuleEndpoints(ISAPSystem system, ISAPModule module)
    {
        _logger.LogDebug("Available endpoints for system {SystemId}, module {ModuleId}:", 
            system.SystemId, module.ModuleId);

        foreach (var endpoint in module.Endpoints)
        {
            var routePattern = $"/api/{system.SystemId}/{module.ModuleId}{endpoint.Path}";
            _logger.LogDebug("  {Method} {RoutePattern}", endpoint.Method, routePattern);
        }
    }
}