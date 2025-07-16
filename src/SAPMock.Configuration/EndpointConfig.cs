namespace SAPMock.Configuration;

/// <summary>
/// Configuration settings for a SAP endpoint.
/// </summary>
public class EndpointConfig
{
    /// <summary>
    /// Gets or sets the path of the endpoint (e.g., "/sap/opu/rest/service").
    /// </summary>
    public string Path { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the HTTP method for this endpoint (e.g., GET, POST, PUT, DELETE).
    /// </summary>
    public string Method { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the request type name for this endpoint.
    /// </summary>
    public string RequestType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the response type name for this endpoint.
    /// </summary>
    public string ResponseType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the handler type name that processes requests to this endpoint.
    /// </summary>
    public string HandlerType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether this endpoint is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;
}