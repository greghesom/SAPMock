# SAP Mock Service for .NET Aspire

A comprehensive SAP mocking service designed for .NET Aspire applications, enabling developers to test SAP-dependent services without connecting to actual SAP systems.

## 🎯 Overview

The SAP Mock Service provides a flexible, configurable solution for simulating SAP system responses during development and testing. Built specifically for .NET Aspire orchestration, it allows teams to work independently with consistent mock data while supporting custom extensions for specific testing scenarios.

## 🚀 Key Features

- **Multi-System Support** - Mock multiple SAP systems simultaneously (ECC, S/4HANA, etc.)
- **Module-Based Architecture** - Supports common SAP modules (MM, SD, FI) with easy extensibility
- **Layered Data Management** - Shared common data with developer-specific overrides
- **Dynamic Configuration** - Switch between data sets and configurations via Aspire
- **SAP-Authentic Responses** - Mimics real SAP response formats and behaviors
- **Error Simulation** - Test failure scenarios and edge cases with configurable error types and probabilities
- **Full Aspire Integration** - Seamless orchestration with dependent services
- **File-Based Mock Data** - Easy to version control and share across teams

## 🔧 Error Simulation

Test how your applications handle various SAP integration failure scenarios with comprehensive error simulation capabilities.

### Error Types and HTTP Status Codes

- **Timeout** (408): Simulates request timeouts with configurable delays
- **Authorization** (401): Simulates authentication/authorization failures  
- **Business** (400): Simulates business logic validation errors
- **System** (500): Simulates system-level errors

### Usage Examples

#### Force Errors with X-SAP-Mock-Error Header

```bash
# Simple error type
curl -H "X-SAP-Mock-Error: Timeout" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001

# JSON configuration for advanced scenarios
curl -H 'X-SAP-Mock-Error: {"ErrorType":2,"CustomMessage":"Invalid material ID","SAPErrorCode":"MM_INVALID_ID"}' \
  http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001
```

#### Probability-based Error Configuration

Create configuration files in `data/errors/{systemId}/{moduleId}/` directory:

**Example**: `data/errors/ERP01/MM/_materials_{id}.json`
```json
[
  {
    "ErrorType": 0,
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
    "ErrorType": 2,
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

### SAP-style Error Response Format

When an error is simulated, responses follow SAP conventions:

```json
{
  "Code": "MM_TIMEOUT",
  "Message": "Material service timeout",
  "Severity": "Error",
  "Category": "Technical",
  "Details": {
    "service": "MaterialService",
    "operation": "GetMaterial",
    "timeout_ms": 5000
  },
  "Timestamp": "2024-01-01T12:00:00.000Z"
}
```

### Error Configuration Reference

| Property | Type | Description |
|----------|------|-------------|
| `ErrorType` | int | Error type: 0=Timeout, 1=Authorization, 2=Business, 3=System |
| `Probability` | double | Probability of error occurrence (0.0 to 1.0) |
| `DelayMs` | int | Delay in milliseconds before response |
| `CustomMessage` | string | Custom error message |
| `SAPErrorCode` | string | SAP-specific error code |
| `AdditionalDetails` | object | Additional error context |

### Error Logging and Persistence

All simulated errors are logged with detailed information and persisted to `data/errors/logs/error-simulation-{date}.json` for analysis and debugging.

### Testing Error Scenarios

```bash
# Test timeout handling
curl -H "X-SAP-Mock-Error: Timeout" http://localhost:5204/api/ERP01/MM/materials/MAT001

# Test authorization failure
curl -H "X-SAP-Mock-Error: Authorization" http://localhost:5204/api/ERP01/SD/orders/ORD001

# Test business logic error
curl -H "X-SAP-Mock-Error: Business" http://localhost:5204/api/ERP01/MM/materials/INVALID_ID

# Test system error
curl -H "X-SAP-Mock-Error: System" http://localhost:5204/api/ERP01/MM/materials/MAT001
```

## 🏗️ Architecture

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   Your Service  │────▶│  SAP Mock API   │────▶│   Mock Data     │
│  (.NET Aspire)  │     │  (.NET Core 8)  │     │  (JSON Files)   │
└─────────────────┘     └─────────────────┘     └─────────────────┘
                               │
                               ▼
                        ┌─────────────────┐
                        │  Configuration  │
                        │  (Systems/Mods) │
                        └─────────────────┘
```

## 💡 Use Cases

- **Local Development** - Develop without VPN or SAP system access
- **Integration Testing** - Consistent, repeatable test scenarios
- **CI/CD Pipelines** - Automated testing without SAP dependencies
- **Demo Environments** - Showcase functionality without production data
- **Performance Testing** - Load test without impacting SAP systems
- **Error Scenario Testing** - Simulate SAP errors, timeouts, and failures with configurable probabilities

## 🏃 Quick Start

```bash
# Clone the repository
git clone https://github.com/yourorg/sap-mock-service.git

# Start with Aspire
dotnet run --project ./AppHost

# Access the mock service
curl http://localhost:5000/api/ERP-DEV/MM/materials/MATERIAL-001

# Test error simulation
curl -H "X-SAP-Mock-Error: Timeout" http://localhost:5000/api/ERP-DEV/MM/materials/MATERIAL-001
```

## 📦 What's Included

- Core mocking framework with plugin architecture
- Pre-built handlers for Materials Management (MM), Sales & Distribution (SD), and Finance (FI)
- Sample mock data for common SAP entities
- **Error simulation system** with configurable error types, probabilities, and SAP-style responses
- Configuration templates for different SAP systems
- Aspire orchestration setup
- Comprehensive documentation and examples

## 🤝 Contributing

This service is designed to be extended by development teams. Add your own modules, customize responses, and share improvements back to the team. See our [Contributing Guide](CONTRIBUTING.md) for details.

## 📄 License

[Your License Here]

---

Built with ❤️ for .NET developers working with SAP integrations.


# SAP Mocking Service for .NET Aspire - Comprehensive Implementation Guide

## Architecture Overview

### Solution Structure
```
SAPMockingService/
├── src/
│   ├── SAPMock.Core/               # Core abstractions and interfaces
│   ├── SAPMock.Api/                # ASP.NET Core Web API
│   ├── SAPMock.Data/               # Mock data models and repositories
│   ├── SAPMock.Configuration/      # Configuration management
│   └── SAPMock.ServiceDefaults/    # Aspire service defaults
├── data/
│   ├── common/                     # Shared mock data
│   │   ├── materials/
│   │   ├── customers/
│   │   └── orders/
│   └── extensions/                 # Developer-specific extensions
├── config/
│   ├── systems/                    # SAP system configurations
│   │   ├── erp-prod.json
│   │   ├── erp-dev.json
│   │   └── s4hana.json
│   └── modules/                    # Module-specific configs
│       ├── mm.json                 # Materials Management
│       ├── sd.json                 # Sales & Distribution
│       └── fi.json                 # Finance
└── AppHost/                        # Aspire Host project
```

## Core Components

### 1. Core Abstractions (SAPMock.Core)

```csharp
// ISAPSystem.cs
public interface ISAPSystem
{
    string SystemId { get; }
    string Name { get; }
    string Type { get; } // ERP, S/4HANA, etc.
    Dictionary<string, string> ConnectionParameters { get; }
}

// ISAPModule.cs
public interface ISAPModule
{
    string ModuleId { get; }
    string Name { get; }
    string SystemId { get; }
    List<ISAPEndpoint> Endpoints { get; }
}

// ISAPEndpoint.cs
public interface ISAPEndpoint
{
    string Path { get; }
    HttpMethod Method { get; }
    Type RequestType { get; }
    Type ResponseType { get; }
    Func<object, Task<object>> Handler { get; }
}

// IMockDataProvider.cs
public interface IMockDataProvider
{
    Task<T> GetDataAsync<T>(string key, string system, string module);
    Task<IEnumerable<T>> GetCollectionAsync<T>(string key, string system, string module);
    Task SaveDataAsync<T>(string key, T data, string system, string module);
}
```

### 2. Configuration Management (SAPMock.Configuration)

```csharp
// SAPMockConfiguration.cs
public class SAPMockConfiguration
{
    public string DataPath { get; set; } = "./data";
    public string ConfigPath { get; set; } = "./config";
    public bool EnableExtensions { get; set; } = true;
    public string ActiveProfile { get; set; } = "default";
    public List<SAPSystemConfig> Systems { get; set; } = new();
}

// ConfigurationService.cs
public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public ConfigurationService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public async Task<SAPSystemConfig> GetSystemConfigAsync(string systemId)
    {
        var configPath = Path.Combine(_configuration["SAPMock:ConfigPath"], "systems", $"{systemId}.json");
        var json = await File.ReadAllTextAsync(configPath);
        return JsonSerializer.Deserialize<SAPSystemConfig>(json);
    }

    public async Task<List<ModuleConfig>> GetModulesForSystemAsync(string systemId)
    {
        var modulesPath = Path.Combine(_configuration["SAPMock:ConfigPath"], "modules");
        var modules = new List<ModuleConfig>();
        
        foreach (var file in Directory.GetFiles(modulesPath, "*.json"))
        {
            var json = await File.ReadAllTextAsync(file);
            var module = JsonSerializer.Deserialize<ModuleConfig>(json);
            if (module.SupportedSystems.Contains(systemId))
            {
                modules.Add(module);
            }
        }
        
        return modules;
    }
}
```

### 3. Mock Data Provider (SAPMock.Data)

```csharp
// FileBasedMockDataProvider.cs
public class FileBasedMockDataProvider : IMockDataProvider
{
    private readonly SAPMockConfiguration _config;
    private readonly ILogger<FileBasedMockDataProvider> _logger;

    public FileBasedMockDataProvider(IOptions<SAPMockConfiguration> config, ILogger<FileBasedMockDataProvider> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task<T> GetDataAsync<T>(string key, string system, string module)
    {
        // Try extension data first if enabled
        if (_config.EnableExtensions)
        {
            var extensionPath = BuildPath("extensions", system, module, $"{key}.json");
            if (File.Exists(extensionPath))
            {
                _logger.LogInformation("Loading extension data for {Key}", key);
                return await LoadJsonAsync<T>(extensionPath);
            }
        }

        // Fall back to common data
        var commonPath = BuildPath("common", system, module, $"{key}.json");
        if (File.Exists(commonPath))
        {
            return await LoadJsonAsync<T>(commonPath);
        }

        _logger.LogWarning("No mock data found for {Key} in {System}/{Module}", key, system, module);
        return default(T);
    }

    private string BuildPath(params string[] segments)
    {
        return Path.Combine(_config.DataPath, Path.Combine(segments));
    }

    private async Task<T> LoadJsonAsync<T>(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json);
    }
}
```

### 4. Web API Implementation (SAPMock.Api)

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add SAP Mock services
builder.Services.Configure<SAPMockConfiguration>(
    builder.Configuration.GetSection("SAPMock"));
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
builder.Services.AddSingleton<IMockDataProvider, FileBasedMockDataProvider>();
builder.Services.AddSingleton<ISAPSystemRegistry, SAPSystemRegistry>();

// Add module handlers
builder.Services.AddScoped<MaterialsManagementHandler>();
builder.Services.AddScoped<SalesDistributionHandler>();
builder.Services.AddScoped<FinanceHandler>();

// Dynamic endpoint registration
builder.Services.AddHostedService<EndpointRegistrationService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// System info endpoint
app.MapGet("/api/systems", async (ISAPSystemRegistry registry) =>
{
    var systems = await registry.GetAllSystemsAsync();
    return Results.Ok(systems);
});

// Dynamic SAP endpoints
app.MapSAPEndpoints();

app.Run();
```

```csharp
// EndpointMapper.cs
public static class EndpointMapper
{
    public static void MapSAPEndpoints(this WebApplication app)
    {
        var registry = app.Services.GetRequiredService<ISAPSystemRegistry>();
        var systems = registry.GetAllSystemsAsync().GetAwaiter().GetResult();

        foreach (var system in systems)
        {
            foreach (var module in system.Modules)
            {
                foreach (var endpoint in module.Endpoints)
                {
                    var path = $"/api/{system.SystemId}/{module.ModuleId}{endpoint.Path}";
                    
                    switch (endpoint.Method)
                    {
                        case HttpMethod.Get:
                            app.MapGet(path, endpoint.Handler);
                            break;
                        case HttpMethod.Post:
                            app.MapPost(path, endpoint.Handler);
                            break;
                        // Add other HTTP methods as needed
                    }
                }
            }
        }
    }
}
```

### 5. Module Handlers Example

```csharp
// MaterialsManagementHandler.cs
public class MaterialsManagementHandler : ISAPModuleHandler
{
    private readonly IMockDataProvider _dataProvider;
    private readonly ILogger<MaterialsManagementHandler> _logger;

    public MaterialsManagementHandler(IMockDataProvider dataProvider, ILogger<MaterialsManagementHandler> logger)
    {
        _dataProvider = dataProvider;
        _logger = logger;
    }

    public List<ISAPEndpoint> GetEndpoints(string systemId)
    {
        return new List<ISAPEndpoint>
        {
            new SAPEndpoint
            {
                Path = "/materials/{materialId}",
                Method = HttpMethod.Get,
                Handler = async (context) => await GetMaterialAsync(context, systemId)
            },
            new SAPEndpoint
            {
                Path = "/materials",
                Method = HttpMethod.Get,
                Handler = async (context) => await GetMaterialsAsync(context, systemId)
            },
            new SAPEndpoint
            {
                Path = "/materials",
                Method = HttpMethod.Post,
                Handler = async (context) => await CreateMaterialAsync(context, systemId)
            }
        };
    }

    private async Task<IResult> GetMaterialAsync(HttpContext context, string systemId)
    {
        var materialId = context.Request.RouteValues["materialId"]?.ToString();
        var material = await _dataProvider.GetDataAsync<Material>(materialId, systemId, "MM");
        
        if (material == null)
        {
            return Results.NotFound(new { error = $"Material {materialId} not found" });
        }
        
        return Results.Ok(material);
    }
}
```

### 6. Aspire Integration (AppHost)

```csharp
// Program.cs in AppHost project
var builder = DistributedApplication.CreateBuilder(args);

// Configure SAP Mock Service with different data paths for different environments
var sapMockService = builder.AddProject<Projects.SAPMock_Api>("sap-mock")
    .WithEnvironment("SAPMock__DataPath", builder.Configuration["SAPMock:DataPath"] ?? "./data")
    .WithEnvironment("SAPMock__ConfigPath", builder.Configuration["SAPMock:ConfigPath"] ?? "./config")
    .WithEnvironment("SAPMock__EnableExtensions", "true")
    .WithEnvironment("SAPMock__ActiveProfile", builder.Configuration["SAPMock:Profile"] ?? "default");

// Add dependent services that will use the SAP Mock
var orderService = builder.AddProject<Projects.OrderService>("order-service")
    .WithReference(sapMockService)
    .WithEnvironment("SAP__BaseUrl", sapMockService.GetEndpoint("http"));

var inventoryService = builder.AddProject<Projects.InventoryService>("inventory-service")
    .WithReference(sapMockService)
    .WithEnvironment("SAP__BaseUrl", sapMockService.GetEndpoint("http"));

builder.Build().Run();
```

## Mock Data Structure

### Common Data Example
```json
// data/common/ERP-DEV/MM/MATERIAL-001.json
{
  "materialNumber": "MATERIAL-001",
  "description": "Test Material",
  "materialType": "FERT",
  "baseUnit": "EA",
  "materialGroup": "01",
  "weight": {
    "gross": 10.5,
    "net": 10.0,
    "unit": "KG"
  },
  "dimensions": {
    "length": 100,
    "width": 50,
    "height": 30,
    "unit": "CM"
  }
}
```

### System Configuration Example
```json
// config/systems/erp-dev.json
{
  "systemId": "ERP-DEV",
  "name": "ERP Development System",
  "type": "ECC",
  "version": "6.0",
  "modules": ["MM", "SD", "FI"],
  "connectionParameters": {
    "client": "100",
    "language": "EN"
  }
}
```

### Module Configuration Example
```json
// config/modules/mm.json
{
  "moduleId": "MM",
  "name": "Materials Management",
  "supportedSystems": ["ERP-DEV", "ERP-PROD", "S4HANA"],
  "endpoints": [
    {
      "name": "GetMaterial",
      "path": "/materials/{id}",
      "method": "GET"
    },
    {
      "name": "ListMaterials",
      "path": "/materials",
      "method": "GET"
    }
  ]
}
```

### Error Simulation Data Examples

#### Materials Management Error Configuration
```json
// data/errors/ERP01/MM/_materials_{id}.json
[
  {
    "ErrorType": 0,
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
    "ErrorType": 2,
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

#### Sales & Distribution Error Configuration
```json
// data/errors/ERP01/SD/_orders_{id}.json
[
  {
    "ErrorType": 1,
    "Probability": 0.02,
    "DelayMs": 1000,
    "CustomMessage": "Insufficient authorization for sales data",
    "SAPErrorCode": "SD_AUTH_FAILED",
    "AdditionalDetails": {
      "required_role": "SAP_SD_DISPLAY",
      "user_roles": ["SAP_USER", "SAP_MM_DISPLAY"]
    }
  },
  {
    "ErrorType": 3,
    "Probability": 0.03,
    "DelayMs": 2000,
    "CustomMessage": "Sales system temporarily unavailable",
    "SAPErrorCode": "SD_SYSTEM_DOWN",
    "AdditionalDetails": {
      "system_status": "maintenance",
      "estimated_recovery": "2024-01-01T14:30:00Z"
    }
  }
]
```

#### Error Response Example
```json
// Typical error response returned by the mock service
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

#### Error Simulation Log Example
```json
// data/errors/logs/error-simulation-2024-01-01.json
{
  "timestamp": "2024-01-01T12:00:00.000Z",
  "system": "ERP01",
  "module": "MM",
  "endpoint": "/materials/MATERIAL-001",
  "errorType": "Timeout",
  "httpStatus": 408,
  "errorCode": "MM_TIMEOUT",
  "message": "Material service timeout",
  "delayMs": 5000,
  "triggeredBy": "probability",
  "probability": 0.1,
  "requestId": "req-123456",
  "userAgent": "curl/7.68.0"
}
```

## Usage in Development

### 1. Local Development with Aspire
```bash
# Start with default configuration
dotnet run --project ./AppHost

# Start with custom data path
dotnet run --project ./AppHost -- --SAPMock:DataPath="./custom-data" --SAPMock:Profile="integration-test"
```

### 2. Switching Configurations in launchSettings.json
```json
{
  "profiles": {
    "Development": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "SAPMock__DataPath": "./data",
        "SAPMock__Profile": "dev"
      }
    },
    "Integration": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "SAPMock__DataPath": "./test-data",
        "SAPMock__Profile": "integration"
      }
    }
  }
}
```

### 3. Consuming the Mock Service
```csharp
// In your service that needs SAP data
public class OrderService
{
    private readonly HttpClient _httpClient;
    
    public OrderService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["SAP:BaseUrl"]);
    }
    
    public async Task<Material> GetMaterialAsync(string materialId)
    {
        var response = await _httpClient.GetAsync($"/api/ERP-DEV/MM/materials/{materialId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Material>();
    }
}
```

### 4. Testing with Error Simulation
```csharp
// Test error handling in your service
public class OrderServiceTests
{
    [Test]
    public async Task GetMaterialAsync_ShouldHandleTimeout()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-SAP-Mock-Error", "Timeout");
        var service = new OrderService(httpClient, configuration);
        
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => 
            service.GetMaterialAsync("MATERIAL-001"));
    }
    
    [Test]
    public async Task GetMaterialAsync_ShouldHandleAuthFailure()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-SAP-Mock-Error", "Authorization");
        var service = new OrderService(httpClient, configuration);
        
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => 
            service.GetMaterialAsync("MATERIAL-001"));
    }
}
```

### 5. Error Simulation in CI/CD
```bash
#!/bin/bash
# Integration test script with error simulation

# Test normal operation
response=$(curl -s -w "%{http_code}" http://localhost:5000/api/ERP-DEV/MM/materials/MAT001)
if [[ "$response" != *"200"* ]]; then
    echo "Normal operation test failed"
    exit 1
fi

# Test timeout handling
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Timeout" http://localhost:5000/api/ERP-DEV/MM/materials/MAT001)
if [[ "$response" != *"408"* ]]; then
    echo "Timeout test failed"
    exit 1
fi

# Test authorization error handling
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Authorization" http://localhost:5000/api/ERP-DEV/MM/materials/MAT001)
if [[ "$response" != *"401"* ]]; then
    echo "Authorization test failed"
    exit 1
fi

echo "All error simulation tests passed!"
```

## Extension Points

### 1. Custom Mock Data Providers
```csharp
public class DatabaseMockDataProvider : IMockDataProvider
{
    // Implementation that reads from a database instead of files
}
```

### 2. Dynamic Response Generation
```csharp
public class DynamicMaterialHandler : ISAPModuleHandler
{
    public async Task<IResult> GenerateMaterialAsync(HttpContext context, string systemId)
    {
        // Generate random but consistent material data based on ID
        var materialId = context.Request.RouteValues["materialId"]?.ToString();
        var material = GenerateMaterial(materialId);
        return Results.Ok(material);
    }
}
```

### 3. Request/Response Logging Middleware
```csharp
public class SAPMockLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SAPMockLoggingMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("SAP Mock Request: {Method} {Path}", 
            context.Request.Method, context.Request.Path);
        
        await _next(context);
        
        _logger.LogInformation("SAP Mock Response: {StatusCode}", 
            context.Response.StatusCode);
    }
}
```

## Best Practices

1. **Data Organization**: Keep mock data organized by system/module/entity
2. **Version Control**: Include common mock data in version control, exclude developer-specific extensions
3. **Configuration Profiles**: Use different profiles for different testing scenarios
4. **Health Checks**: Implement health checks for each SAP system mock
5. **Documentation**: Document available endpoints and mock data structure
6. **Validation**: Add request validation to match SAP's behavior
7. **Error Simulation**: 
   - Use realistic error probabilities (0.01-0.1) for testing
   - Configure different error types per endpoint based on real SAP behavior
   - Include error simulation in automated testing pipelines
   - Monitor error simulation logs for analysis
   - Test error recovery mechanisms regularly
