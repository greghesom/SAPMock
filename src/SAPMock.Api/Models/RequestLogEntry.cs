namespace SAPMock.Api.Models;

/// <summary>
/// Represents a logged HTTP request for monitoring purposes.
/// </summary>
public class RequestLogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string System { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public string RequestBody { get; set; } = string.Empty;
    public string ResponseBody { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string RemoteIpAddress { get; set; } = string.Empty;
}