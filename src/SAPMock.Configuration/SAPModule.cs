using SAPMock.Core;

namespace SAPMock.Configuration;

/// <summary>
/// Concrete implementation of ISAPModule for configuration purposes.
/// </summary>
internal class SAPModule : ISAPModule
{
    /// <summary>
    /// Gets the unique identifier for the SAP module.
    /// </summary>
    public string ModuleId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the display name of the SAP module.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the identifier of the SAP system this module belongs to.
    /// </summary>
    public string SystemId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the collection of endpoints available in this module.
    /// </summary>
    public IEnumerable<ISAPEndpoint> Endpoints { get; set; } = new List<ISAPEndpoint>();
}