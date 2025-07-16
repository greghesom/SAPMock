using Microsoft.Extensions.DependencyInjection;
using SAPMock.Configuration;
using SAPMock.Core;

namespace SAPMock.Data.Extensions;

/// <summary>
/// Extension methods for configuring services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the FileBasedMockDataProvider to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The SAP Mock configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddFileBasedMockDataProvider(
        this IServiceCollection services, 
        SAPMockConfiguration configuration)
    {
        services.AddSingleton<IMockDataProvider>(provider =>
            new FileBasedMockDataProvider(configuration.DataPath, configuration.EnableExtensions));
        
        return services;
    }
}