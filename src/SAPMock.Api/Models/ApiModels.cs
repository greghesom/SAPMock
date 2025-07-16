namespace SAPMock.Api.Models;

/// <summary>
/// Data transfer object for SAP system information.
/// </summary>
public class SystemResponse
{
    public string SystemId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, string> ConnectionParameters { get; set; } = new();
}

/// <summary>
/// Data transfer object for SAP module information.
/// </summary>
public class ModuleResponse
{
    public string ModuleId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SystemId { get; set; } = string.Empty;
    public List<EndpointResponse> Endpoints { get; set; } = new();
}

/// <summary>
/// Data transfer object for SAP endpoint information.
/// </summary>
public class EndpointResponse
{
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string ResponseType { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for health check response.
/// </summary>
public class HealthResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Dictionary<string, bool> Systems { get; set; } = new();
}