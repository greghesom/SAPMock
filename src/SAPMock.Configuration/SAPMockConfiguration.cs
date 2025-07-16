namespace SAPMock.Configuration;

/// <summary>
/// Configuration settings for the SAP Mock system.
/// </summary>
public class SAPMockConfiguration
{
    /// <summary>
    /// Gets or sets the path to the data directory.
    /// </summary>
    public string DataPath { get; set; } = "data";
    
    /// <summary>
    /// Gets or sets the path to the configuration directory.
    /// </summary>
    public string ConfigPath { get; set; } = "config";
    
    /// <summary>
    /// Gets or sets a value indicating whether extensions are enabled.
    /// </summary>
    public bool EnableExtensions { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the active profile name.
    /// </summary>
    public string ActiveProfile { get; set; } = "default";
    
    /// <summary>
    /// Gets or sets the collection of SAP systems.
    /// </summary>
    public List<SAPSystemConfig> Systems { get; set; } = new();
}
