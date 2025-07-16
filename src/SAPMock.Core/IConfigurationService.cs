namespace SAPMock.Core;

/// <summary>
/// Provides configuration management capabilities for SAP systems and modules.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Asynchronously loads system configuration from JSON files.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the configuration.</returns>
    Task<IEnumerable<ISAPSystem>> LoadSystemConfigurationAsync();
    
    /// <summary>
    /// Asynchronously loads module configurations and filters by supported systems.
    /// </summary>
    /// <param name="systemId">The system identifier to filter modules by.</param>
    /// <returns>A task that represents the asynchronous operation, containing the modules for the specified system.</returns>
    Task<IEnumerable<ISAPModule>> LoadModuleConfigurationsAsync(string systemId);
    
    /// <summary>
    /// Validates the configuration for completeness and correctness.
    /// </summary>
    /// <returns>A task that represents the asynchronous validation operation, returning true if valid.</returns>
    Task<bool> ValidateConfigurationAsync();
}