using Microsoft.Extensions.Logging;
using SAPMock.Core;
using System.Text.Json;

namespace SAPMock.Configuration.Services;

/// <summary>
/// Service for handling error simulation in SAP Mock endpoints.
/// </summary>
public class ErrorSimulationService : IErrorSimulationService
{
    private readonly ILogger<ErrorSimulationService> _logger;
    private readonly Random _random;
    private readonly string _errorDataPath;

    /// <summary>
    /// Initializes a new instance of the ErrorSimulationService.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The SAP Mock configuration.</param>
    public ErrorSimulationService(ILogger<ErrorSimulationService> logger, SAPMockConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _random = new Random();
        _errorDataPath = Path.Combine(configuration.DataPath, "errors");
    }

    /// <summary>
    /// Determines if an error should be simulated for the given endpoint.
    /// </summary>
    public async Task<ErrorSimulationConfig?> ShouldSimulateErrorAsync(string systemId, string moduleId, string endpointPath, IDictionary<string, string> headers)
    {
        // Check for X-SAP-Mock-Error header to force specific error
        if (headers.TryGetValue("X-SAP-Mock-Error", out var errorHeader))
        {
            var errorConfig = await ParseErrorHeaderAsync(errorHeader);
            if (errorConfig != null)
            {
                _logger.LogInformation("Force simulating error from X-SAP-Mock-Error header: {ErrorType}", errorConfig.ErrorType);
                return errorConfig;
            }
        }

        // Load error configurations for this endpoint
        var errorConfigs = await LoadErrorConfigurationsAsync(systemId, moduleId, endpointPath);
        
        // Check each error configuration for probability-based simulation
        foreach (var config in errorConfigs)
        {
            if (config.Probability > 0 && _random.NextDouble() < config.Probability)
            {
                _logger.LogInformation("Randomly simulating error: {ErrorType} with probability {Probability}", 
                    config.ErrorType, config.Probability);
                return config;
            }
        }

        return null;
    }

    /// <summary>
    /// Creates a SAP-style error response for the given error configuration.
    /// </summary>
    public SAPMockErrorResponse CreateErrorResponse(ErrorSimulationConfig errorConfig)
    {
        return errorConfig.ErrorType switch
        {
            ErrorType.Timeout => new SAPMockErrorResponse
            {
                Code = errorConfig.SAPErrorCode ?? "TIMEOUT",
                Message = errorConfig.CustomMessage ?? "Request timed out",
                Severity = "Error",
                Category = "Technical",
                Details = errorConfig.AdditionalDetails ?? new Dictionary<string, object>
                {
                    ["timeout_ms"] = errorConfig.DelayMs
                }
            },
            ErrorType.Authorization => new SAPMockErrorResponse
            {
                Code = errorConfig.SAPErrorCode ?? "AUTHORIZATION_FAILED",
                Message = errorConfig.CustomMessage ?? "Authorization failed",
                Severity = "Error",
                Category = "Authorization",
                Details = errorConfig.AdditionalDetails ?? new Dictionary<string, object>
                {
                    ["reason"] = "Invalid credentials or insufficient permissions"
                }
            },
            ErrorType.Business => new SAPMockErrorResponse
            {
                Code = errorConfig.SAPErrorCode ?? "BUSINESS_ERROR",
                Message = errorConfig.CustomMessage ?? "Business logic validation failed",
                Severity = "Error",
                Category = "Business",
                Details = errorConfig.AdditionalDetails ?? new Dictionary<string, object>
                {
                    ["context"] = "Data validation or business rule violation"
                }
            },
            ErrorType.System => new SAPMockErrorResponse
            {
                Code = errorConfig.SAPErrorCode ?? "SYSTEM_ERROR",
                Message = errorConfig.CustomMessage ?? "System error occurred",
                Severity = "Error",
                Category = "System",
                Details = errorConfig.AdditionalDetails ?? new Dictionary<string, object>
                {
                    ["internal_error"] = "Internal system failure"
                }
            },
            _ => new SAPMockErrorResponse
            {
                Code = "UNKNOWN_ERROR",
                Message = "Unknown error occurred",
                Severity = "Error",
                Category = "Unknown"
            }
        };
    }

    /// <summary>
    /// Logs the simulated error.
    /// </summary>
    public async Task LogSimulatedErrorAsync(string systemId, string moduleId, string endpointPath, ErrorSimulationConfig errorConfig, SAPMockErrorResponse errorResponse)
    {
        var logEntry = new
        {
            Timestamp = DateTime.UtcNow,
            SystemId = systemId,
            ModuleId = moduleId,
            EndpointPath = endpointPath,
            ErrorType = errorConfig.ErrorType.ToString(),
            ErrorCode = errorResponse.Code,
            Message = errorResponse.Message,
            Probability = errorConfig.Probability,
            DelayMs = errorConfig.DelayMs
        };

        _logger.LogWarning("SAP Mock Error Simulation: {ErrorType} on {SystemId}/{ModuleId}{EndpointPath} - Code: {ErrorCode}, Message: {Message}",
            errorConfig.ErrorType, systemId, moduleId, endpointPath, errorResponse.Code, errorResponse.Message);

        // Optionally persist error simulation logs to file
        await PersistErrorLogAsync(logEntry);
    }

    /// <summary>
    /// Parses the X-SAP-Mock-Error header to create an error configuration.
    /// </summary>
    private async Task<ErrorSimulationConfig?> ParseErrorHeaderAsync(string? headerValue)
    {
        if (string.IsNullOrEmpty(headerValue))
            return null;

        try
        {
            // Support both simple format (just error type) and JSON format
            if (Enum.TryParse<ErrorType>(headerValue, true, out var errorType))
            {
                return new ErrorSimulationConfig
                {
                    ErrorType = errorType,
                    Probability = 1.0 // Always trigger when header is present
                };
            }

            // Try to parse as JSON for more complex configurations
            var config = JsonSerializer.Deserialize<ErrorSimulationConfig>(headerValue);
            if (config != null)
            {
                config.Probability = 1.0; // Always trigger when header is present
                return config;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse X-SAP-Mock-Error header: {HeaderValue}", headerValue);
        }

        return null;
    }

    /// <summary>
    /// Loads error configurations for a specific endpoint.
    /// </summary>
    private async Task<List<ErrorSimulationConfig>> LoadErrorConfigurationsAsync(string systemId, string moduleId, string endpointPath)
    {
        var configs = new List<ErrorSimulationConfig>();

        try
        {
            var configPath = Path.Combine(_errorDataPath, systemId, moduleId, $"{endpointPath.Replace("/", "_")}.json");
            
            if (File.Exists(configPath))
            {
                var json = await File.ReadAllTextAsync(configPath);
                var loadedConfigs = JsonSerializer.Deserialize<List<ErrorSimulationConfig>>(json);
                if (loadedConfigs != null)
                {
                    configs.AddRange(loadedConfigs);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load error configurations for {SystemId}/{ModuleId}{EndpointPath}", 
                systemId, moduleId, endpointPath);
        }

        return configs;
    }

    /// <summary>
    /// Persists error log entries to file.
    /// </summary>
    private async Task PersistErrorLogAsync(object logEntry)
    {
        try
        {
            var logsPath = Path.Combine(_errorDataPath, "logs");
            Directory.CreateDirectory(logsPath);

            var logFile = Path.Combine(logsPath, $"error-simulation-{DateTime.UtcNow:yyyy-MM-dd}.json");
            var json = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
            
            await File.AppendAllTextAsync(logFile, json + Environment.NewLine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist error simulation log");
        }
    }
}