# Error Simulation in SAP Mock

The SAP Mock service supports error simulation to help you test how your applications handle various failure scenarios that might occur when integrating with SAP systems.

## Features

### Error Types
- **Timeout**: Simulates request timeouts
- **Authorization**: Simulates authentication/authorization failures
- **Business**: Simulates business logic validation errors
- **System**: Simulates system-level errors

### Trigger Methods
1. **X-SAP-Mock-Error Header**: Force specific errors on demand
2. **Probability-based**: Random errors based on configured probabilities
3. **Configuration Files**: Per-endpoint error configurations

## Usage

### 1. Using the X-SAP-Mock-Error Header

Force a specific error by adding the `X-SAP-Mock-Error` header to your request:

```bash
# Simple error type
curl -H "X-SAP-Mock-Error: Timeout" http://localhost:5000/api/ERP01/MM/materials/MAT001

# JSON configuration for advanced scenarios
curl -H "X-SAP-Mock-Error: {\"ErrorType\":\"Business\",\"CustomMessage\":\"Invalid material ID\",\"SAPErrorCode\":\"MM_INVALID_ID\"}" \
  http://localhost:5000/api/ERP01/MM/materials/MAT001
```

### 2. Configuration Files

Create error configuration files in the `data/errors/{systemId}/{moduleId}/` directory:

**Example**: `data/errors/ERP01/MM/_materials_{id}.json`
```json
[
  {
    "ErrorType": "Timeout",
    "Probability": 0.1,
    "DelayMs": 5000,
    "CustomMessage": "Material service timeout",
    "SAPErrorCode": "MM_TIMEOUT",
    "AdditionalDetails": {
      "service": "MaterialService",
      "operation": "GetMaterial"
    }
  },
  {
    "ErrorType": "Business",
    "Probability": 0.05,
    "DelayMs": 0,
    "CustomMessage": "Material not found in system",
    "SAPErrorCode": "MM_NOT_FOUND",
    "AdditionalDetails": {
      "reason": "Material ID does not exist",
      "suggested_action": "Check material ID and try again"
    }
  }
]
```

### 3. Error Response Format

When an error is simulated, the response follows SAP-style error format:

```json
{
  "Code": "MM_TIMEOUT",
  "Message": "Material service timeout",
  "Severity": "Error",
  "Category": "Technical",
  "Target": null,
  "Details": {
    "service": "MaterialService",
    "operation": "GetMaterial",
    "timeout_ms": 5000
  },
  "Timestamp": "2024-01-01T12:00:00.000Z"
}
```

### 4. HTTP Status Codes

Different error types return appropriate HTTP status codes:

- **Timeout**: 408 Request Timeout
- **Authorization**: 401 Unauthorized
- **Business**: 400 Bad Request
- **System**: 500 Internal Server Error

## Configuration Reference

### ErrorSimulationConfig Properties

| Property | Type | Description |
|----------|------|-------------|
| `ErrorType` | ErrorType | The type of error to simulate |
| `Probability` | double | Probability of error (0.0 to 1.0) |
| `DelayMs` | int | Delay in milliseconds (for timeouts) |
| `CustomMessage` | string | Custom error message |
| `SAPErrorCode` | string | SAP-specific error code |
| `AdditionalDetails` | object | Additional error details |

### Error Types

- `Timeout`: Simulates request timeouts
- `Authorization`: Simulates auth failures
- `Business`: Simulates business logic errors
- `System`: Simulates system-level errors

## Examples

### 1. Test Timeout Handling
```bash
curl -H "X-SAP-Mock-Error: Timeout" \
  http://localhost:5000/api/ERP01/MM/materials/MAT001
```

### 2. Test Authorization Failure
```bash
curl -H "X-SAP-Mock-Error: Authorization" \
  http://localhost:5000/api/ERP01/SD/orders/ORD001
```

### 3. Test Business Error
```bash
curl -H "X-SAP-Mock-Error: Business" \
  http://localhost:5000/api/ERP01/MM/materials/INVALID_ID
```

### 4. Test System Error
```bash
curl -H "X-SAP-Mock-Error: System" \
  http://localhost:5000/api/ERP01/MM/materials/MAT001
```

## Error Logging

All simulated errors are logged with detailed information:
- Error type and configuration
- System, module, and endpoint information
- Request timestamp
- Error response details

Error simulation logs are also persisted to `data/errors/logs/error-simulation-{date}.json` for analysis.

## Best Practices

1. **Use in Testing**: Enable error simulation in test environments only
2. **Realistic Probabilities**: Use low probabilities (0.01-0.1) for realistic testing
3. **Meaningful Messages**: Provide clear, actionable error messages
4. **Log Analysis**: Review error simulation logs to understand system behavior
5. **Progressive Testing**: Start with simple errors and gradually increase complexity

## Integration with CI/CD

Use error simulation in automated tests:

```bash
# Test timeout handling
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Timeout" http://localhost:5000/api/ERP01/MM/materials/MAT001)
if [ "$response" != "408" ]; then
  echo "Expected 408 for timeout, got $response"
  exit 1
fi

# Test authorization handling
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Authorization" http://localhost:5000/api/ERP01/MM/materials/MAT001)
if [ "$response" != "401" ]; then
  echo "Expected 401 for auth error, got $response"
  exit 1
fi
```

This allows you to verify that your application handles SAP integration errors gracefully.