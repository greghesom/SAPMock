using SAPMock.Api.Models;

namespace SAPMock.Api.Services;

/// <summary>
/// Interface for monitoring and logging HTTP requests.
/// </summary>
public interface IRequestMonitorService
{
    /// <summary>
    /// Logs a request entry.
    /// </summary>
    /// <param name="entry">The request log entry.</param>
    void LogRequest(RequestLogEntry entry);

    /// <summary>
    /// Gets recent requests up to the specified count.
    /// </summary>
    /// <param name="count">Maximum number of requests to return.</param>
    /// <returns>Collection of recent request log entries.</returns>
    IEnumerable<RequestLogEntry> GetRecentRequests(int count = 100);

    /// <summary>
    /// Notifies all connected clients about a new request asynchronously.
    /// </summary>
    /// <param name="entry">The request log entry.</param>
    Task NotifyRequestAsync(RequestLogEntry entry);

    /// <summary>
    /// Event triggered when a new request is logged.
    /// </summary>
    event EventHandler<RequestLogEntry>? OnNewRequest;

    /// <summary>
    /// Clears all logged requests.
    /// </summary>
    void ClearRequests();

    /// <summary>
    /// Gets the total count of logged requests.
    /// </summary>
    int GetTotalRequestCount();
}