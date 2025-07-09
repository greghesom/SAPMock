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
7. **Error Simulation**: Include ability to simulate SAP errors and timeouts
