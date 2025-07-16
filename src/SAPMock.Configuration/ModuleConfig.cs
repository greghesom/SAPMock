namespace SAPMock.Configuration;

/// <summary>
/// Configuration settings for a SAP module.
/// </summary>
public class ModuleConfig
{
    /// <summary>
    /// Gets or sets the unique identifier for the SAP module.
    /// </summary>
    public string ModuleId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the display name of the SAP module.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the identifier of the SAP system this module belongs to.
    /// </summary>
    public string SystemId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the endpoints available in this module.
    /// </summary>
    public List<EndpointConfig> Endpoints { get; set; } = new();
    
    /// <summary>
    /// Gets or sets a value indicating whether this module is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the configuration file path for this module.
    /// </summary>
    public string ConfigFile { get; set; } = string.Empty;
}