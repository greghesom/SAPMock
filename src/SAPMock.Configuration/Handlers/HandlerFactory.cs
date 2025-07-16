using SAPMock.Core;
using Microsoft.Extensions.DependencyInjection;

namespace SAPMock.Configuration.Handlers;

/// <summary>
/// Factory for creating SAP module handlers.
/// </summary>
public class HandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the HandlerFactory.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public HandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Creates a handler based on the handler type name.
    /// </summary>
    /// <param name="handlerType">The handler type name.</param>
    /// <param name="systemId">The system ID.</param>
    /// <returns>The created handler or null if not found.</returns>
    public ISAPModuleHandler? CreateHandler(string handlerType, string systemId)
    {
        return handlerType switch
        {
            "MMHandler" or "MaterialsHandler" or "MaterialsManagementHandler" => 
                _serviceProvider.GetService<MaterialsManagementHandler>(),
            "SDHandler" or "SalesDistributionHandler" => 
                _serviceProvider.GetService<SalesDistributionHandler>(),
            _ => null
        };
    }
}