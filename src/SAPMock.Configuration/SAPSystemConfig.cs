namespace SAPMock.Configuration;

/// <summary>
/// Configuration settings for a SAP system.
/// </summary>
public class SAPSystemConfig
{
    /// <summary>
    /// Gets or sets the unique identifier for the SAP system.
    /// </summary>
    public string SystemId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the display name of the SAP system.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the type of the SAP system (e.g., ERP, CRM, etc.).
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the connection parameters required to connect to the SAP system.
    /// </summary>
    public Dictionary<string, string> ConnectionParameters { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the modules available in this SAP system.
    /// </summary>
    public List<ModuleConfig> Modules { get; set; } = new();
    
    /// <summary>
    /// Gets or sets a value indicating whether this system is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;
}