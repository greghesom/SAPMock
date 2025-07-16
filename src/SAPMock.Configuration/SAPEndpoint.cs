using SAPMock.Core;

namespace SAPMock.Configuration;

/// <summary>
/// Concrete implementation of ISAPEndpoint for configuration purposes.
/// </summary>
public class SAPEndpoint : ISAPEndpoint
{
    /// <summary>
    /// Gets the path of the endpoint (e.g., "/sap/opu/rest/service").
    /// </summary>
    public string Path { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the HTTP method for this endpoint (e.g., GET, POST, PUT, DELETE).
    /// </summary>
    public string Method { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the type of the request object expected by this endpoint.
    /// </summary>
    public Type RequestType { get; set; } = typeof(object);
    
    /// <summary>
    /// Gets the type of the response object returned by this endpoint.
    /// </summary>
    public Type ResponseType { get; set; } = typeof(object);
    
    /// <summary>
    /// Gets the handler function that processes requests to this endpoint.
    /// </summary>
    public Func<object, Task<object>> Handler { get; set; } = async (req) => await Task.FromResult<object>(new { });
    
    /// <summary>
    /// Gets the error simulation configurations for this endpoint.
    /// </summary>
    public IEnumerable<ErrorSimulationConfig> ErrorSimulations { get; set; } = new List<ErrorSimulationConfig>();
}