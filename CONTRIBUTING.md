# Contributing to SAP Mock Service

## Overview

The SAP Mock Service is designed to be extended by development teams. This guide will help you add new modules, customize responses, and contribute improvements back to the project.

## Adding New Modules

### Step 1: Create Data Models

First, create your data models in the `SAPMock.Configuration/Models` directory:

```csharp
// SAPMock.Configuration/Models/YourModule/YourEntity.cs
namespace SAPMock.Configuration.Models.YourModule;

public class YourEntity
{
    public string EntityId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    // Add more properties as needed
}

public class YourEntityResponse
{
    public YourEntity Entity { get; set; } = new();
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class YourEntityListResponse
{
    public List<YourEntity> Entities { get; set; } = new();
    public int TotalCount { get; set; }
    public bool Success { get; set; }
}

public class CreateYourEntityRequest
{
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    // Add validation attributes as needed
}
```

### Step 2: Create Module Handler

Create a handler class in `SAPMock.Configuration/Handlers`:

```csharp
// SAPMock.Configuration/Handlers/YourModuleHandler.cs
using SAPMock.Configuration.Models.YourModule;
using SAPMock.Core;

namespace SAPMock.Configuration.Handlers;

public class YourModuleHandler : ISAPModuleHandler
{
    private readonly IMockDataProvider _mockDataProvider;
    private readonly string _systemId;
    private readonly List<ISAPEndpoint> _endpoints;

    public YourModuleHandler(IMockDataProvider mockDataProvider, string systemId)
    {
        _mockDataProvider = mockDataProvider ?? throw new ArgumentNullException(nameof(mockDataProvider));
        _systemId = systemId ?? throw new ArgumentNullException(nameof(systemId));
        _endpoints = InitializeEndpoints();
    }

    public IEnumerable<ISAPEndpoint> GetEndpoints(string systemId)
    {
        return _endpoints;
    }

    private List<ISAPEndpoint> InitializeEndpoints()
    {
        return new List<ISAPEndpoint>
        {
            // GET /your-entities/{id}
            new SAPEndpoint
            {
                Path = "/your-entities/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(YourEntityResponse),
                Handler = GetYourEntityHandler,
                ErrorSimulations = new List<ErrorSimulationConfig>
                {
                    new ErrorSimulationConfig
                    {
                        ErrorType = ErrorType.Timeout,
                        Probability = 0.02,
                        DelayMs = 5000,
                        CustomMessage = "Your entity service timeout",
                        SAPErrorCode = "YM_TIMEOUT"
                    }
                }
            },
            // GET /your-entities
            new SAPEndpoint
            {
                Path = "/your-entities",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(YourEntityListResponse),
                Handler = ListYourEntitiesHandler
            },
            // POST /your-entities
            new SAPEndpoint
            {
                Path = "/your-entities",
                Method = "POST",
                RequestType = typeof(CreateYourEntityRequest),
                ResponseType = typeof(YourEntityResponse),
                Handler = CreateYourEntityHandler
            }
        };
    }

    private async Task<object> GetYourEntityHandler(HttpRequest request)
    {
        var entityId = request.RouteValues["id"]?.ToString();
        if (string.IsNullOrEmpty(entityId))
        {
            return new YourEntityResponse
            {
                Success = false,
                Message = "Entity ID is required"
            };
        }

        var entity = await _mockDataProvider.GetDataAsync<YourEntity>(
            $"{_systemId}/YM/{entityId}"
        );

        if (entity == null)
        {
            return new YourEntityResponse
            {
                Success = false,
                Message = $"Entity {entityId} not found"
            };
        }

        return new YourEntityResponse
        {
            Entity = entity,
            Success = true,
            Message = "Entity retrieved successfully"
        };
    }

    private async Task<object> ListYourEntitiesHandler(HttpRequest request)
    {
        var entities = await _mockDataProvider.GetDataAsync<List<YourEntity>>(
            $"{_systemId}/YM/entities"
        );

        entities ??= new List<YourEntity>();

        return new YourEntityListResponse
        {
            Entities = entities,
            TotalCount = entities.Count,
            Success = true
        };
    }

    private async Task<object> CreateYourEntityHandler(HttpRequest request)
    {
        var createRequest = await request.ReadFromJsonAsync<CreateYourEntityRequest>();
        if (createRequest == null)
        {
            return new YourEntityResponse
            {
                Success = false,
                Message = "Invalid request body"
            };
        }

        var newEntity = new YourEntity
        {
            EntityId = $"YE{DateTime.Now:yyyyMMddHHmmss}",
            Description = createRequest.Description,
            Status = createRequest.Status,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "SYSTEM"
        };

        // In a real implementation, you might save this to the data provider
        // await _mockDataProvider.SaveDataAsync(newEntity, _systemId, "YM", newEntity.EntityId);

        return new YourEntityResponse
        {
            Entity = newEntity,
            Success = true,
            Message = "Entity created successfully"
        };
    }
}
```

### Step 3: Create Mock Data

Create mock data files in the appropriate directory structure:

```bash
# Create directory structure
mkdir -p data/common/ERP01/YM
mkdir -p data/common/ERP-DEV/YM
mkdir -p data/common/S4HANA/YM
```

```json
// data/common/ERP01/YM/entities.json
[
  {
    "entityId": "YE001",
    "description": "Test Entity 1",
    "status": "Active",
    "createdOn": "2024-01-01T10:00:00Z",
    "createdBy": "SYSTEM"
  },
  {
    "entityId": "YE002",
    "description": "Test Entity 2",
    "status": "Inactive",
    "createdOn": "2024-01-02T11:00:00Z",
    "createdBy": "SYSTEM"
  }
]
```

```json
// data/common/ERP01/YM/YE001.json
{
  "entityId": "YE001",
  "description": "Test Entity 1",
  "status": "Active",
  "createdOn": "2024-01-01T10:00:00Z",
  "createdBy": "SYSTEM"
}
```

### Step 4: Register Module in Configuration

Update system configuration files to include your new module:

```json
// config/system-ERP01.json
{
  "systemId": "ERP01",
  "name": "SAP ERP Development",
  "type": "ERP",
  "enabled": true,
  "modules": [
    {
      "moduleId": "YM",
      "name": "Your Module",
      "systemId": "ERP01",
      "enabled": true,
      "configFile": "module-ERP01-YM.json",
      "endpoints": [
        {
          "path": "/your-entities/{id}",
          "method": "GET",
          "requestType": "object",
          "responseType": "YourEntityResponse",
          "handlerType": "YourModuleHandler",
          "enabled": true
        },
        {
          "path": "/your-entities",
          "method": "GET",
          "requestType": "object",
          "responseType": "YourEntityListResponse",
          "handlerType": "YourModuleHandler",
          "enabled": true
        },
        {
          "path": "/your-entities",
          "method": "POST",
          "requestType": "CreateYourEntityRequest",
          "responseType": "YourEntityResponse",
          "handlerType": "YourModuleHandler",
          "enabled": true
        }
      ]
    }
  ]
}
```

Create a module-specific configuration file:

```json
// config/module-ERP01-YM.json
{
  "moduleId": "YM",
  "name": "Your Module",
  "systemId": "ERP01",
  "enabled": true,
  "endpoints": [
    {
      "path": "/your-entities/{id}",
      "method": "GET",
      "requestType": "object",
      "responseType": "YourEntityResponse",
      "handlerType": "YourModuleHandler",
      "enabled": true
    },
    {
      "path": "/your-entities",
      "method": "GET",
      "requestType": "object",
      "responseType": "YourEntityListResponse",
      "handlerType": "YourModuleHandler",
      "enabled": true
    },
    {
      "path": "/your-entities",
      "method": "POST",
      "requestType": "CreateYourEntityRequest",
      "responseType": "YourEntityResponse",
      "handlerType": "YourModuleHandler",
      "enabled": true
    }
  ]
}
```

### Step 5: Register Handler in DI Container

Update the DI registration to include your new handler:

```csharp
// In Program.cs or wherever handlers are registered
services.AddScoped<YourModuleHandler>();
```

### Step 6: Test Your Module

```bash
# Test getting all entities
curl -X GET "http://localhost:5204/api/ERP01/YM/your-entities" \
  -H "Content-Type: application/json"

# Test getting specific entity
curl -X GET "http://localhost:5204/api/ERP01/YM/your-entities/YE001" \
  -H "Content-Type: application/json"

# Test creating entity
curl -X POST "http://localhost:5204/api/ERP01/YM/your-entities" \
  -H "Content-Type: application/json" \
  -d '{
    "description": "New Test Entity",
    "status": "Active"
  }'

# Test error simulation
curl -H "X-SAP-Mock-Error: Timeout" \
  "http://localhost:5204/api/ERP01/YM/your-entities/YE001"
```

## Customizing Existing Modules

### Override Mock Data

Create extension data files to override existing data:

```bash
# Create extension directory
mkdir -p data/extensions/ERP01/MM

# Copy and modify existing data
cp data/common/ERP01/MM/materials.json data/extensions/ERP01/MM/materials.json
# Edit the extension file with your custom data
```

### Add Custom Endpoints

You can extend existing handlers by creating derived classes:

```csharp
public class ExtendedMaterialsHandler : MaterialsManagementHandler
{
    public ExtendedMaterialsHandler(IMockDataProvider mockDataProvider, string systemId) 
        : base(mockDataProvider, systemId)
    {
    }

    protected override List<ISAPEndpoint> InitializeEndpoints()
    {
        var endpoints = base.InitializeEndpoints();
        
        // Add custom endpoints
        endpoints.Add(new SAPEndpoint
        {
            Path = "/materials/{id}/custom-operation",
            Method = "POST",
            RequestType = typeof(CustomOperationRequest),
            ResponseType = typeof(CustomOperationResponse),
            Handler = CustomOperationHandler
        });

        return endpoints;
    }

    private async Task<object> CustomOperationHandler(HttpRequest request)
    {
        // Your custom implementation
        return new CustomOperationResponse { Success = true };
    }
}
```

## Adding Error Simulation

### Configure Error Simulation for Your Module

Create error configuration files:

```json
// data/errors/ERP01/YM/_your-entities_{id}.json
[
  {
    "ErrorType": 0,
    "Probability": 0.05,
    "DelayMs": 3000,
    "CustomMessage": "Your entity service timeout",
    "SAPErrorCode": "YM_TIMEOUT",
    "AdditionalDetails": {
      "service": "YourEntityService",
      "operation": "GetYourEntity"
    }
  },
  {
    "ErrorType": 2,
    "Probability": 0.02,
    "DelayMs": 0,
    "CustomMessage": "Your entity validation failed",
    "SAPErrorCode": "YM_VALIDATION_ERROR",
    "AdditionalDetails": {
      "reason": "Invalid entity status",
      "suggested_action": "Check entity status and try again"
    }
  }
]
```

### Add Error Simulation to Endpoints

Include error simulation in your endpoint definitions:

```csharp
new SAPEndpoint
{
    Path = "/your-entities/{id}",
    Method = "GET",
    RequestType = typeof(object),
    ResponseType = typeof(YourEntityResponse),
    Handler = GetYourEntityHandler,
    ErrorSimulations = new List<ErrorSimulationConfig>
    {
        new ErrorSimulationConfig
        {
            ErrorType = ErrorType.Timeout,
            Probability = 0.02,
            DelayMs = 5000,
            CustomMessage = "Your entity service timeout",
            SAPErrorCode = "YM_TIMEOUT"
        },
        new ErrorSimulationConfig
        {
            ErrorType = ErrorType.Business,
            Probability = 0.01,
            DelayMs = 0,
            CustomMessage = "Your entity not found",
            SAPErrorCode = "YM_NOT_FOUND"
        }
    }
}
```

## Testing Your Contributions

### Unit Tests

Create unit tests for your handlers:

```csharp
// Tests/YourModuleHandlerTests.cs
[TestFixture]
public class YourModuleHandlerTests
{
    private Mock<IMockDataProvider> _mockDataProvider;
    private YourModuleHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockDataProvider = new Mock<IMockDataProvider>();
        _handler = new YourModuleHandler(_mockDataProvider.Object, "ERP01");
    }

    [Test]
    public async Task GetYourEntityHandler_ReturnsEntity_WhenEntityExists()
    {
        // Arrange
        var entity = new YourEntity { EntityId = "YE001", Description = "Test Entity" };
        _mockDataProvider.Setup(x => x.GetDataAsync<YourEntity>("ERP01/YM/YE001"))
            .ReturnsAsync(entity);

        var request = new Mock<HttpRequest>();
        request.Setup(x => x.RouteValues).Returns(new RouteValueDictionary { ["id"] = "YE001" });

        // Act
        var result = await _handler.GetYourEntityHandler(request.Object);

        // Assert
        Assert.IsInstanceOf<YourEntityResponse>(result);
        var response = (YourEntityResponse)result;
        Assert.IsTrue(response.Success);
        Assert.AreEqual("YE001", response.Entity.EntityId);
    }
}
```

### Integration Tests

Create integration tests to verify end-to-end functionality:

```csharp
// Tests/YourModuleIntegrationTests.cs
[TestFixture]
public class YourModuleIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task GetYourEntity_ReturnsOk_WhenEntityExists()
    {
        // Act
        var response = await _client.GetAsync("/api/ERP01/YM/your-entities/YE001");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<YourEntityResponse>(content);
        
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
```

## Code Quality Guidelines

### Naming Conventions

- **Handlers**: Use `{ModuleName}Handler` pattern
- **Models**: Use descriptive names following SAP conventions
- **Endpoints**: Use REST conventions with plural nouns
- **Files**: Use kebab-case for JSON files, PascalCase for C# files

### Error Handling

- Always return meaningful error messages
- Use appropriate HTTP status codes
- Include SAP-style error codes for authenticity
- Log errors for debugging purposes

### Performance Considerations

- Keep mock data files under 1MB for optimal performance
- Use async/await for all I/O operations
- Implement caching for frequently accessed data
- Consider memory usage when loading large datasets

### Documentation

- Document all public APIs with XML comments
- Include examples in your documentation
- Update this guide when adding new patterns
- Create module-specific README files when needed

## Contributing Back to the Project

### Pull Request Process

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-module`)
3. Commit your changes (`git commit -am 'Add YourModule support'`)
4. Push to the branch (`git push origin feature/your-module`)
5. Create a Pull Request

### PR Guidelines

- Include comprehensive tests for new functionality
- Update documentation for any API changes
- Follow existing code style and patterns
- Include example usage in your PR description

### Code Review Checklist

- [ ] Code follows established patterns
- [ ] All tests pass
- [ ] Documentation is updated
- [ ] Error handling is implemented
- [ ] Performance impact is considered
- [ ] Security implications are addressed

## Getting Help

- **GitHub Issues**: Report bugs or request features
- **Documentation**: Check existing documentation first
- **Code Examples**: Look at existing handlers for patterns
- **Community**: Engage with other contributors

## License

By contributing to this project, you agree that your contributions will be licensed under the same license as the project.