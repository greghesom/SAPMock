using SAPMock.Core;

namespace SAPMock.Configuration;

/// <summary>
/// Concrete implementation of ISAPSystem for configuration purposes.
/// </summary>
public class SAPSystem : ISAPSystem
{
    /// <summary>
    /// Gets the unique identifier for the SAP system.
    /// </summary>
    public string SystemId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the display name of the SAP system.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the type of the SAP system (e.g., ERP, CRM, etc.).
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the connection parameters required to connect to the SAP system.
    /// </summary>
    public Dictionary<string, string> ConnectionParameters { get; set; } = new();
}