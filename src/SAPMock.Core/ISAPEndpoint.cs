namespace SAPMock.Core;

/// <summary>
/// Represents an endpoint within a SAP module, defining its path, method, and request/response handling.
/// </summary>
public interface ISAPEndpoint
{
    /// <summary>
    /// Gets the path of the endpoint (e.g., "/sap/opu/rest/service").
    /// </summary>
    string Path { get; }
    
    /// <summary>
    /// Gets the HTTP method for this endpoint (e.g., GET, POST, PUT, DELETE).
    /// </summary>
    string Method { get; }
    
    /// <summary>
    /// Gets the type of the request object expected by this endpoint.
    /// </summary>
    Type RequestType { get; }
    
    /// <summary>
    /// Gets the type of the response object returned by this endpoint.
    /// </summary>
    Type ResponseType { get; }
    
    /// <summary>
    /// Gets the handler function that processes requests to this endpoint.
    /// </summary>
    Func<object, Task<object>> Handler { get; }
}