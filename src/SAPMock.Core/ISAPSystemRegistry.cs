namespace SAPMock.Core;

/// <summary>
/// Provides registration and retrieval capabilities for SAP systems.
/// </summary>
public interface ISAPSystemRegistry
{
    /// <summary>
    /// Asynchronously registers a new SAP system in the registry.
    /// </summary>
    /// <param name="system">The SAP system to register.</param>
    /// <returns>A task that represents the asynchronous registration operation.</returns>
    Task RegisterSystem(ISAPSystem system);
    
    /// <summary>
    /// Asynchronously retrieves a SAP system by its identifier.
    /// </summary>
    /// <param name="systemId">The unique identifier of the SAP system to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing the SAP system if found.</returns>
    Task<ISAPSystem?> GetSystem(string systemId);
    
    /// <summary>
    /// Asynchronously retrieves all registered SAP systems.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing all registered SAP systems.</returns>
    Task<IEnumerable<ISAPSystem>> GetAllSystemsAsync();
}