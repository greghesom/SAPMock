using System.ComponentModel.DataAnnotations;

namespace SAPMock.Core;

/// <summary>
/// Represents different types of errors that can be simulated in SAP Mock.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Simulates a timeout error.
    /// </summary>
    Timeout,
    
    /// <summary>
    /// Simulates an authorization error.
    /// </summary>
    Authorization,
    
    /// <summary>
    /// Simulates a business logic error.
    /// </summary>
    Business,
    
    /// <summary>
    /// Simulates a system error.
    /// </summary>
    System
}

/// <summary>
/// Configuration for error simulation on a specific endpoint.
/// </summary>
public class ErrorSimulationConfig
{
    /// <summary>
    /// Gets or sets the error type to simulate.
    /// </summary>
    public ErrorType ErrorType { get; set; }
    
    /// <summary>
    /// Gets or sets the probability of this error occurring (0.0 to 1.0).
    /// </summary>
    [Range(0.0, 1.0)]
    public double Probability { get; set; } = 0.0;
    
    /// <summary>
    /// Gets or sets the delay in milliseconds for timeout errors.
    /// </summary>
    public int DelayMs { get; set; } = 5000;
    
    /// <summary>
    /// Gets or sets the custom error message.
    /// </summary>
    public string? CustomMessage { get; set; }
    
    /// <summary>
    /// Gets or sets the SAP error code.
    /// </summary>
    public string? SAPErrorCode { get; set; }
    
    /// <summary>
    /// Gets or sets additional error details.
    /// </summary>
    public Dictionary<string, object>? AdditionalDetails { get; set; }
}

/// <summary>
/// SAP-style error response format for error simulation.
/// </summary>
public class SAPMockErrorResponse
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public string Severity { get; set; } = "Error";
    
    /// <summary>
    /// Gets or sets the error category.
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the target field or component.
    /// </summary>
    public string? Target { get; set; }
    
    /// <summary>
    /// Gets or sets additional error details.
    /// </summary>
    public Dictionary<string, object>? Details { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Interface for error simulation service.
/// </summary>
public interface IErrorSimulationService
{
    /// <summary>
    /// Determines if an error should be simulated for the given endpoint.
    /// </summary>
    /// <param name="systemId">The SAP system ID.</param>
    /// <param name="moduleId">The SAP module ID.</param>
    /// <param name="endpointPath">The endpoint path.</param>
    /// <param name="headers">The request headers.</param>
    /// <returns>The error simulation config if an error should be simulated, null otherwise.</returns>
    Task<ErrorSimulationConfig?> ShouldSimulateErrorAsync(string systemId, string moduleId, string endpointPath, IDictionary<string, string> headers);
    
    /// <summary>
    /// Creates a SAP-style error response for the given error configuration.
    /// </summary>
    /// <param name="errorConfig">The error configuration.</param>
    /// <returns>The SAP error response.</returns>
    SAPMockErrorResponse CreateErrorResponse(ErrorSimulationConfig errorConfig);
    
    /// <summary>
    /// Logs the simulated error.
    /// </summary>
    /// <param name="systemId">The SAP system ID.</param>
    /// <param name="moduleId">The SAP module ID.</param>
    /// <param name="endpointPath">The endpoint path.</param>
    /// <param name="errorConfig">The error configuration.</param>
    /// <param name="errorResponse">The error response.</param>
    Task LogSimulatedErrorAsync(string systemId, string moduleId, string endpointPath, ErrorSimulationConfig errorConfig, SAPMockErrorResponse errorResponse);
}