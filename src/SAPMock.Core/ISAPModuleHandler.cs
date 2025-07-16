namespace SAPMock.Core;

/// <summary>
/// Provides handling capabilities for SAP modules, including endpoint retrieval.
/// </summary>
public interface ISAPModuleHandler
{
    /// <summary>
    /// Retrieves all endpoints for a specific SAP system.
    /// </summary>
    /// <param name="systemId">The unique identifier of the SAP system.</param>
    /// <returns>An enumerable collection of endpoints for the specified system.</returns>
    IEnumerable<ISAPEndpoint> GetEndpoints(string systemId);
}