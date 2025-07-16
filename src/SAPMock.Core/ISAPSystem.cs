namespace SAPMock.Core;

/// <summary>
/// Represents a SAP system with its identification and connection information.
/// </summary>
public interface ISAPSystem
{
    /// <summary>
    /// Gets the unique identifier for the SAP system.
    /// </summary>
    string SystemId { get; }
    
    /// <summary>
    /// Gets the display name of the SAP system.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets the type of the SAP system (e.g., ERP, CRM, etc.).
    /// </summary>
    string Type { get; }
    
    /// <summary>
    /// Gets the connection parameters required to connect to the SAP system.
    /// </summary>
    Dictionary<string, string> ConnectionParameters { get; }
}
