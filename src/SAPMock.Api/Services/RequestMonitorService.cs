using SAPMock.Api.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace SAPMock.Api.Services;

/// <summary>
/// Service for monitoring and logging HTTP requests with real-time notifications.
/// </summary>
public class RequestMonitorService : IRequestMonitorService
{
    private readonly ConcurrentQueue<RequestLogEntry> _requests = new();
    private readonly IHubContext<RequestHub> _hubContext;
    private readonly ILogger<RequestMonitorService> _logger;
    private const int MaxRequests = 1000;

    public event EventHandler<RequestLogEntry>? OnNewRequest;

    public RequestMonitorService(IHubContext<RequestHub> hubContext, ILogger<RequestMonitorService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public void LogRequest(RequestLogEntry entry)
    {
        _requests.Enqueue(entry);

        // Maintain maximum request count (FIFO)
        while (_requests.Count > MaxRequests)
        {
            _requests.TryDequeue(out _);
        }

        // Trigger event
        OnNewRequest?.Invoke(this, entry);

        // Notify clients asynchronously without blocking
        _ = Task.Run(async () =>
        {
            try
            {
                await NotifyRequestAsync(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to notify clients about new request");
            }
        });
    }

    public IEnumerable<RequestLogEntry> GetRecentRequests(int count = 100)
    {
        return _requests.ToArray()
            .TakeLast(count)
            .Reverse(); // Most recent first
    }

    public async Task NotifyRequestAsync(RequestLogEntry entry)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveRequest", entry);
    }

    public void ClearRequests()
    {
        while (_requests.TryDequeue(out _)) { }
        _logger.LogInformation("All request logs cleared");
    }

    public int GetTotalRequestCount()
    {
        return _requests.Count;
    }
}

/// <summary>
/// SignalR Hub for real-time request monitoring.
/// </summary>
public class RequestHub : Hub
{
    private readonly ILogger<RequestHub> _logger;

    public RequestHub(ILogger<RequestHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogDebug("Client connected to RequestHub: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogDebug("Client disconnected from RequestHub: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}