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

### High-Level Architecture
```
┌─────────────────────────────────────────────────────────────────────────────┐
│                          .NET Aspire AppHost                                │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐              │
│  │   Your Service  │  │ Example Service │  │   Other Apps    │              │
│  │                 │  │                 │  │                 │              │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘              │
│           │                     │                     │                     │
│           └─────────────────────┼─────────────────────┘                     │
│                                 │                                           │
│                                 ▼                                           │
│  ┌─────────────────────────────────────────────────────────────────────────┐ │
│  │                    SAP Mock API (.NET Core 9)                          │ │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐         │ │
│  │  │       MM        │  │       SD        │  │       FI        │         │ │
│  │  │   Materials     │  │ Sales & Distrib │  │   Financial     │         │ │
│  │  │  Management     │  │                 │  │  Accounting     │         │ │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘         │ │
│  │                                                                         │ │
│  │  ┌─────────────────────────────────────────────────────────────────────┐ │ │
│  │  │                Error Simulation Engine                             │ │ │
│  │  │  • Timeout Errors    • Business Errors                            │ │ │
│  │  │  • Auth Errors       • System Errors                              │ │ │
│  │  └─────────────────────────────────────────────────────────────────────┘ │ │
│  └─────────────────────────────────────────────────────────────────────────┘ │
│                                 │                                           │
│                                 ▼                                           │
│  ┌─────────────────────────────────────────────────────────────────────────┐ │
│  │                    Configuration System                                 │ │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐         │ │
│  │  │   System Config │  │  Module Config  │  │ Endpoint Config │         │ │
│  │  │   (ERP01, etc.) │  │   (MM, SD, FI)  │  │   (Paths, etc.) │         │ │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘         │ │
│  └─────────────────────────────────────────────────────────────────────────┘ │
│                                 │                                           │
│                                 ▼                                           │
│  ┌─────────────────────────────────────────────────────────────────────────┐ │
│  │                     Mock Data Provider                                  │ │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐         │ │
│  │  │   Extensions    │  │    Profiles     │  │     Common      │         │ │
│  │  │  (Developer     │  │   (Environment  │  │   (Baseline     │         │ │
│  │  │   Overrides)    │  │    Specific)    │  │     Data)       │         │ │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘         │ │
│  └─────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Component Details

#### 1. SAP Mock API
- **Framework**: ASP.NET Core 9 Web API
- **Purpose**: Exposes SAP-like REST endpoints
- **Port**: 5204 (HTTP), 7000 (HTTPS)
- **Features**: Dynamic endpoint registration, error simulation, health checks

#### 2. Module Handlers
- **Materials Management (MM)**: Material CRUD operations
- **Sales & Distribution (SD)**: Customer, Sales Order, Delivery, Invoice operations
- **Financial Accounting (FI)**: General ledger and financial data
- **Extensible**: Easy to add new modules

#### 3. Configuration System
- **System Configs**: Define SAP systems (ERP01, S4HANA, etc.)
- **Module Configs**: Define available modules per system
- **Endpoint Configs**: Define paths, methods, and handlers
- **File-based**: JSON configuration files

#### 4. Mock Data Provider
- **Layered Architecture**: Extensions → Profiles → Common
- **File-based Storage**: JSON files organized by system/module
- **Caching**: Built-in caching for performance
- **Fallback Logic**: Automatic fallback between layers

#### 5. Error Simulation Engine
- **Multiple Trigger Methods**: Headers, configuration files, probability-based
- **SAP-style Responses**: Authentic error response formats
- **Comprehensive Logging**: Detailed error simulation logs
- **Configurable**: Per-endpoint error configurations

### Data Flow

1. **Request**: Client sends HTTP request to SAP Mock API
2. **Routing**: API routes request to appropriate module handler
3. **Error Check**: Error simulation engine checks for configured errors
4. **Data Retrieval**: Mock data provider fetches data from layered storage
5. **Response**: Handler returns SAP-formatted response
6. **Logging**: Request/response logged for debugging

### Technology Stack

- **.NET 9**: Core framework
- **ASP.NET Core**: Web API framework
- **.NET Aspire**: Orchestration and service management
- **System.Text.Json**: JSON serialization
- **Microsoft.Extensions.Logging**: Logging framework
- **Microsoft.Extensions.Configuration**: Configuration management

## 💡 Use Cases

- **Local Development** - Develop without VPN or SAP system access
- **Integration Testing** - Consistent, repeatable test scenarios
- **CI/CD Pipelines** - Automated testing without SAP dependencies
- **Demo Environments** - Showcase functionality without production data
- **Performance Testing** - Load test without impacting SAP systems
- **Error Scenario Testing** - Simulate SAP errors, timeouts, and failures with configurable probabilities

## 🏃 Getting Started Guide

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022 or VS Code (optional)
- Git for version control

### Step 1: Clone and Setup
```bash
# Clone the repository
git clone https://github.com/greghesom/SAPMock.git
cd SAPMock

# Restore dependencies
dotnet restore

# Build the solution
dotnet build
```

### Step 2: Start the Services
```bash
# Option 1: Using .NET Aspire (Recommended)
dotnet run --project src/SAPMock.AppHost

# Option 2: Run API directly
dotnet run --project src/SAPMock.Api
```

### Step 3: Verify Installation
```bash
# Check service health
curl http://localhost:5204/api/health

# Get available systems
curl http://localhost:5204/api/systems

# Test a sample endpoint
curl http://localhost:5204/api/ERP01/SD/customers
```

### Step 4: Explore the API
- **Aspire Dashboard**: http://localhost:15005 (when using AppHost)
- **API Base URL**: http://localhost:5204
- **Health Check**: http://localhost:5204/api/health
- **Systems Info**: http://localhost:5204/api/systems

### Step 5: Test Error Simulation
```bash
# Test timeout error
curl -H "X-SAP-Mock-Error: Timeout" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001

# Test business error
curl -H "X-SAP-Mock-Error: Business" http://localhost:5204/api/ERP01/SD/customers/INVALID-ID
```

### Step 6: Customize Data (Optional)
```bash
# Create your own data override
mkdir -p data/extensions/ERP01/MM
cp data/common/ERP01/MM/materials.json data/extensions/ERP01/MM/materials.json

# Edit the extension file with your custom data
# The service will automatically use your extension data
```

### Next Steps
- Review the [Complete API Reference](#complete-api-reference) for detailed endpoint documentation
- Check the [Configuration Guide](CONFIGURATION.md) for advanced configuration options
- Explore [Error Simulation](ERROR_SIMULATION.md) for testing failure scenarios
- See [Adding New Modules](#adding-new-modules) to extend the service

## 🏃 Quick Start

```bash
# Clone the repository
git clone https://github.com/greghesom/SAPMock.git

# Start with Aspire
dotnet run --project ./src/SAPMock.AppHost

# Access the mock service
curl http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001

# Test error simulation
curl -H "X-SAP-Mock-Error: Timeout" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001
```

## 📦 What's Included

- Core mocking framework with plugin architecture
- Pre-built handlers for Materials Management (MM), Sales & Distribution (SD), and Finance (FI)
- Sample mock data for common SAP entities
- **Error simulation system** with configurable error types, probabilities, and SAP-style responses
- Configuration templates for different SAP systems
- Aspire orchestration setup
- Comprehensive documentation and examples

## 📚 API Documentation

### Available Endpoints by Module

#### Materials Management (MM)
- `GET /api/{systemId}/MM/materials` - List all materials
- `GET /api/{systemId}/MM/materials/{id}` - Get specific material
- `POST /api/{systemId}/MM/materials` - Create new material
- `PUT /api/{systemId}/MM/materials/{id}` - Update material
- `DELETE /api/{systemId}/MM/materials/{id}` - Delete material

#### Sales & Distribution (SD)
- `GET /api/{systemId}/SD/customers` - List all customers
- `GET /api/{systemId}/SD/customers/{id}` - Get specific customer
- `POST /api/{systemId}/SD/customers` - Create new customer
- `PUT /api/{systemId}/SD/customers/{id}` - Update customer
- `DELETE /api/{systemId}/SD/customers/{id}` - Delete customer
- `GET /api/{systemId}/SD/sales-orders` - List all sales orders
- `GET /api/{systemId}/SD/sales-orders/{id}` - Get specific sales order
- `POST /api/{systemId}/SD/sales-orders` - Create new sales order
- `PUT /api/{systemId}/SD/sales-orders/{id}` - Update sales order
- `DELETE /api/{systemId}/SD/sales-orders/{id}` - Delete sales order
- `GET /api/{systemId}/SD/deliveries` - List all deliveries
- `GET /api/{systemId}/SD/deliveries/{id}` - Get specific delivery
- `POST /api/{systemId}/SD/deliveries` - Create new delivery
- `PUT /api/{systemId}/SD/deliveries/{id}` - Update delivery
- `DELETE /api/{systemId}/SD/deliveries/{id}` - Delete delivery
- `GET /api/{systemId}/SD/invoices` - List all invoices
- `GET /api/{systemId}/SD/invoices/{id}` - Get specific invoice
- `POST /api/{systemId}/SD/invoices` - Create new invoice
- `PUT /api/{systemId}/SD/invoices/{id}` - Update invoice
- `DELETE /api/{systemId}/SD/invoices/{id}` - Delete invoice
- `POST /api/{systemId}/SD/sales-orders/{id}/create-delivery` - Create delivery from sales order
- `POST /api/{systemId}/SD/deliveries/{id}/create-invoice` - Create invoice from delivery

#### Financial Accounting (FI)
- `GET /api/{systemId}/FI/general-ledger` - Get general ledger data

See the [Complete API Reference](#complete-api-reference) below for detailed request/response examples.

## 🤝 Contributing

This service is designed to be extended by development teams. Add your own modules, customize responses, and share improvements back to the team.

### Quick Module Addition

1. **Create Handler**: Implement `ISAPModuleHandler` interface
2. **Add Mock Data**: Create JSON files in `data/common/{system}/{module}/`
3. **Register Module**: Add to system configuration files
4. **Test**: Verify endpoints work correctly

See our comprehensive [Contributing Guide](CONTRIBUTING.md) for detailed instructions on:
- Creating new modules and handlers
- Adding mock data and configurations
- Implementing error simulation
- Testing and code quality guidelines
- Contributing back to the project

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

---

# Complete API Reference

## Materials Management (MM) Module

### Get Material
**GET** `/api/{systemId}/MM/materials/{id}`

Get a specific material by ID.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001" \
  -H "Content-Type: application/json"
```

**Example Response:**
```json
{
  "materialNumber": "MATERIAL-001",
  "description": "Common Test Material 1",
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

### List Materials
**GET** `/api/{systemId}/MM/materials`

Get all materials.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/ERP01/MM/materials" \
  -H "Content-Type: application/json"
```

**Example Response:**
```json
[
  {
    "materialNumber": "MATERIAL-001",
    "description": "Common Test Material 1",
    "materialType": "FERT",
    "baseUnit": "EA",
    "materialGroup": "01"
  },
  {
    "materialNumber": "MATERIAL-002",
    "description": "Common Test Material 2",
    "materialType": "FERT",
    "baseUnit": "EA",
    "materialGroup": "02"
  }
]
```

### Create Material
**POST** `/api/{systemId}/MM/materials`

Create a new material.

**Example Request:**
```bash
curl -X POST "http://localhost:5204/api/ERP01/MM/materials" \
  -H "Content-Type: application/json" \
  -d '{
    "materialNumber": "MATERIAL-NEW",
    "description": "New Test Material",
    "materialType": "FERT",
    "baseUnit": "EA",
    "materialGroup": "01",
    "weight": {
      "gross": 15.0,
      "net": 14.5,
      "unit": "KG"
    }
  }'
```

**Example Response:**
```json
{
  "materialNumber": "MATERIAL-NEW",
  "description": "New Test Material",
  "materialType": "FERT",
  "baseUnit": "EA",
  "materialGroup": "01",
  "weight": {
    "gross": 15.0,
    "net": 14.5,
    "unit": "KG"
  },
  "created": true,
  "timestamp": "2024-01-01T12:00:00Z"
}
```

### Update Material
**PUT** `/api/{systemId}/MM/materials/{id}`

Update an existing material.

**Example Request:**
```bash
curl -X PUT "http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001" \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Updated Test Material",
    "materialType": "FERT",
    "baseUnit": "EA",
    "materialGroup": "01"
  }'
```

### Delete Material
**DELETE** `/api/{systemId}/MM/materials/{id}`

Delete a material.

**Example Request:**
```bash
curl -X DELETE "http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001"
```

## Sales & Distribution (SD) Module

### Get Customer
**GET** `/api/{systemId}/SD/customers/{id}`

Get a specific customer by ID.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/ERP01/SD/customers/CUST001" \
  -H "Content-Type: application/json"
```

**Example Response:**
```json
{
  "customerNumber": "CUST001",
  "name": "ABC Corporation",
  "name2": "Global Division",
  "searchTerm": "ABC CORP",
  "city": "New York",
  "postalCode": "10001",
  "country": "US",
  "region": "NY",
  "street": "123 Main Street",
  "customerGroup": "Z001",
  "salesOrganization": "1000",
  "distributionChannel": "10",
  "division": "00",
  "currency": "USD",
  "paymentTerms": "NET30",
  "creditLimit": 100000.00,
  "telephone": "+1-555-123-4567",
  "email": "orders@abccorp.com",
  "createdOn": "2024-01-15T10:00:00Z",
  "createdBy": "SYSTEM",
  "lastChangedOn": "2024-01-15T10:00:00Z",
  "lastChangedBy": "SYSTEM",
  "deletionFlag": false,
  "blockedFlag": false
}
```

### List Customers
**GET** `/api/{systemId}/SD/customers`

Get all customers.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/ERP01/SD/customers" \
  -H "Content-Type: application/json"
```

### Create Customer
**POST** `/api/{systemId}/SD/customers`

Create a new customer.

**Example Request:**
```bash
curl -X POST "http://localhost:5204/api/ERP01/SD/customers" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "New Customer Corp",
    "city": "Boston",
    "postalCode": "02101",
    "country": "US",
    "region": "MA",
    "street": "456 New Street",
    "customerGroup": "Z001",
    "salesOrganization": "1000",
    "distributionChannel": "10",
    "division": "00",
    "currency": "USD",
    "paymentTerms": "NET30",
    "creditLimit": 50000.00,
    "telephone": "+1-555-000-0000",
    "email": "contact@newcustomer.com"
  }'
```

### Sales Orders

#### Get Sales Order
**GET** `/api/{systemId}/SD/sales-orders/{id}`

Get a specific sales order by ID.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/ERP01/SD/sales-orders/SO001" \
  -H "Content-Type: application/json"
```

#### Create Sales Order
**POST** `/api/{systemId}/SD/sales-orders`

Create a new sales order.

**Example Request:**
```bash
curl -X POST "http://localhost:5204/api/ERP01/SD/sales-orders" \
  -H "Content-Type: application/json" \
  -d '{
    "customerNumber": "CUST001",
    "salesOrganization": "1000",
    "distributionChannel": "10",
    "division": "00",
    "orderType": "TA",
    "items": [
      {
        "materialNumber": "MATERIAL-001",
        "quantity": 10,
        "unit": "EA",
        "plant": "1000"
      }
    ]
  }'
```

#### Create Delivery from Sales Order
**POST** `/api/{systemId}/SD/sales-orders/{id}/create-delivery`

Create a delivery document from a sales order.

**Example Request:**
```bash
curl -X POST "http://localhost:5204/api/ERP01/SD/sales-orders/SO001/create-delivery" \
  -H "Content-Type: application/json" \
  -d '{
    "deliveryDate": "2024-01-15",
    "shippingPoint": "1000"
  }'
```

### Deliveries

#### Get Delivery
**GET** `/api/{systemId}/SD/deliveries/{id}`

Get a specific delivery by ID.

#### Create Invoice from Delivery
**POST** `/api/{systemId}/SD/deliveries/{id}/create-invoice`

Create an invoice document from a delivery.

**Example Request:**
```bash
curl -X POST "http://localhost:5204/api/ERP01/SD/deliveries/DEL001/create-invoice" \
  -H "Content-Type: application/json" \
  -d '{
    "invoiceDate": "2024-01-16",
    "billingType": "F2"
  }'
```

## Financial Accounting (FI) Module

### Get General Ledger
**GET** `/api/{systemId}/FI/general-ledger`

Get general ledger data.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/ERP01/FI/general-ledger" \
  -H "Content-Type: application/json"
```

## Error Simulation Examples

### Force Timeout Error
```bash
curl -H "X-SAP-Mock-Error: Timeout" \
  "http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001"
```

### Force Business Error with Custom Message
```bash
curl -H 'X-SAP-Mock-Error: {"ErrorType":2,"CustomMessage":"Material validation failed","SAPErrorCode":"MM_VALIDATION_ERROR"}' \
  "http://localhost:5204/api/ERP01/MM/materials/INVALID-MATERIAL"
```

### Test Authorization Error
```bash
curl -H "X-SAP-Mock-Error: Authorization" \
  "http://localhost:5204/api/ERP01/SD/customers/CUST001"
```

## System Information

### Get Available Systems
**GET** `/api/systems`

Get all configured SAP systems.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/systems" \
  -H "Content-Type: application/json"
```

### Health Check
**GET** `/api/health`

Check service health status.

**Example Request:**
```bash
curl -X GET "http://localhost:5204/api/health"
```

**Example Response:**
```json
{
  "status": "healthy",
  "timestamp": "2024-01-01T12:00:00Z",
  "systems": {
    "ERP01": true,
    "ERP-DEV": true,
    "ERP-PROD": true,
    "S4HANA": true
  }
}
```

## Adding New Modules

### Step 1: Create Module Handler

Create a new handler class implementing `ISAPModuleHandler`:

```csharp
public class YourModuleHandler : ISAPModuleHandler
{
    private readonly IMockDataProvider _mockDataProvider;
    private readonly string _systemId;

    public YourModuleHandler(IMockDataProvider mockDataProvider, string systemId)
    {
        _mockDataProvider = mockDataProvider;
        _systemId = systemId;
    }

    public IEnumerable<ISAPEndpoint> GetEndpoints(string systemId)
    {
        return new List<ISAPEndpoint>
        {
            new SAPEndpoint
            {
                Path = "/your-entity/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(YourEntityResponse),
                Handler = GetYourEntityHandler
            }
            // Add more endpoints as needed
        };
    }

    private async Task<object> GetYourEntityHandler(HttpRequest request)
    {
        // Your implementation here
        var id = request.RouteValues["id"]?.ToString();
        var entity = await _mockDataProvider.GetDataAsync<YourEntity>(
            $"{_systemId}/YourModule/{id}"
        );
        return entity ?? new { error = "Entity not found" };
    }
}
```

### Step 2: Register Module in Configuration

Add your module to the system configuration file:

```json
{
  "systemId": "ERP01",
  "modules": [
    {
      "moduleId": "YOUR_MODULE",
      "name": "Your Module Name",
      "systemId": "ERP01",
      "enabled": true,
      "endpoints": [
        {
          "path": "/your-entity/{id}",
          "method": "GET",
          "requestType": "object",
          "responseType": "YourEntityResponse",
          "handlerType": "YourModuleHandler",
          "enabled": true
        }
      ]
    }
  ]
}
```

### Step 3: Add Mock Data

Create mock data files in the appropriate directory:

```
data/
├── common/
│   └── ERP01/
│       └── YOUR_MODULE/
│           └── your-entity.json
```

### Step 4: Register Handler in DI Container

Add your handler to the dependency injection container:

```csharp
// In Program.cs or Startup.cs
services.AddScoped<YourModuleHandler>();
```

## Mock Data Structure and Conventions

### Directory Structure
```
data/
├── common/                    # Shared baseline data
│   └── {systemId}/           # e.g., ERP01, ERP-DEV
│       └── {moduleId}/       # e.g., MM, SD, FI
│           └── {entity}.json # e.g., materials.json
├── profiles/                  # Environment-specific data
│   └── {profile}/            # e.g., development, test
│       └── {systemId}/
│           └── {moduleId}/
│               └── {entity}.json
└── extensions/                # Developer-specific overrides
    └── {systemId}/
        └── {moduleId}/
            └── {entity}.json
```

### Data Loading Priority
1. **Extensions** (if enabled) - Developer-specific overrides
2. **Profiles** - Environment-specific data
3. **Common** - Shared baseline data

### Naming Conventions
- **System IDs**: Use UPPERCASE with hyphens (e.g., ERP01, ERP-DEV, S4HANA)
- **Module IDs**: Use SAP standard module codes (MM, SD, FI, etc.)
- **Entity Names**: Use lowercase with hyphens (e.g., materials.json, sales-orders.json)
- **Field Names**: Use camelCase for consistency with JSON standards

### Data Format Guidelines
- All dates should be in ISO 8601 format
- Use consistent field naming across entities
- Include both business keys and technical keys
- Provide realistic test data that mimics SAP structures

## Troubleshooting

### Common Issues

#### 1. Service Not Starting
**Symptom**: Service fails to start or throws exceptions during startup

**Solutions**:
- Check that all required dependencies are installed: `dotnet restore`
- Verify configuration files are valid JSON
- Check that data directory exists and is accessible
- Review application logs for specific error messages

#### 2. Data Not Loading
**Symptom**: Endpoints return empty results or 404 errors

**Solutions**:
- Verify data files exist in the correct directory structure
- Check JSON file format is valid
- Ensure file names match expected patterns
- Verify data path configuration is correct
- Check that EnableExtensions setting matches your setup

#### 3. Endpoints Not Found
**Symptom**: API calls return 404 "Not Found" errors

**Solutions**:
- Verify the endpoint URL format: `/api/{systemId}/{moduleId}/{entity}`
- Check that the system and module are configured in configuration files
- Ensure the service is running on the expected port
- Verify the system ID and module ID are correct

#### 4. Error Simulation Not Working
**Symptom**: Error simulation headers are ignored

**Solutions**:
- Check header format: `X-SAP-Mock-Error: {ErrorType}` or JSON format
- Verify error type is valid (Timeout, Authorization, Business, System)
- Ensure error simulation is enabled in configuration
- Check error configuration files if using probability-based errors

#### 5. Configuration Changes Not Applied
**Symptom**: Configuration changes don't take effect

**Solutions**:
- Restart the service after configuration changes
- Check configuration file syntax is valid JSON
- Verify environment variables are set correctly
- Ensure configuration precedence is correct (command line > env vars > files)

#### 6. Performance Issues
**Symptom**: Slow response times or high memory usage

**Solutions**:
- Check data file sizes - large files may cause performance issues
- Verify caching is enabled in the data provider
- Monitor memory usage and consider reducing data set size
- Check for file system permission issues

### Debug Configuration

Enable detailed logging to troubleshoot issues:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "SAPMock": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Getting Help

1. **Check Logs**: Application logs contain detailed error information
2. **Verify Configuration**: Use the `/health` endpoint to check service status
3. **Test with Curl**: Use the provided curl examples to test endpoints
4. **Review Documentation**: Check this README and individual module documentation
5. **Check GitHub Issues**: Look for similar issues in the repository

### Performance Optimization

1. **Data File Size**: Keep individual JSON files under 1MB for optimal performance
2. **Caching**: Enable caching in the data provider for frequently accessed data
3. **Extensions**: Disable extensions in production if not needed
4. **Selective Loading**: Use profiles to load only necessary data for specific environments

### Development Tips

1. **Use Extensions**: Create extension files for your specific test scenarios
2. **Version Control**: Include common data in version control, exclude extensions
3. **Environment Separation**: Use different profiles for different environments
4. **Test Data**: Create realistic test data that matches your SAP system structure
5. **Documentation**: Document any custom data structures or configurations
