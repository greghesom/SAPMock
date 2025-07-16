namespace SAPMock.Core;

/// <summary>
/// Represents a SAP module within a SAP system, containing multiple endpoints.
/// </summary>
public interface ISAPModule
{
    /// <summary>
    /// Gets the unique identifier for the SAP module.
    /// </summary>
    string ModuleId { get; }
    
    /// <summary>
    /// Gets the display name of the SAP module.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets the identifier of the SAP system this module belongs to.
    /// </summary>
    string SystemId { get; }
    
    /// <summary>
    /// Gets the collection of endpoints available in this module.
    /// </summary>
    IEnumerable<ISAPEndpoint> Endpoints { get; }
}