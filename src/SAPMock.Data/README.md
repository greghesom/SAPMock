# SAPMock.Data

This library provides data storage and retrieval capabilities for mock data in the SAP Mock system.

## FileBasedMockDataProvider

The `FileBasedMockDataProvider` is a file-based implementation of `IMockDataProvider` that loads mock data from JSON files organized in a structured directory layout.

### Features

- **Layered Data System**: Supports "common" and "extensions" layers with automatic fallback
- **Structured File Organization**: Files are organized following the pattern `{DataPath}/{layer}/{system}/{module}/{key}.json`
- **Caching**: Built-in caching mechanism for frequently accessed data
- **Error Handling**: Comprehensive error handling and logging
- **Both Single and Collection Support**: Supports both single object and collection retrieval

### File Structure

```
{DataPath}/
├── extensions/
│   ├── {system}/
│   │   └── {module}/
│   │       └── {key}.json
└── common/
    ├── {system}/
    │   └── {module}/
    │       └── {key}.json
```

### Usage

#### Basic Usage

```csharp
using SAPMock.Data;

// Create provider
var provider = new FileBasedMockDataProvider("./data", enableExtensions: true);

// Retrieve single object
var customer = await provider.GetDataAsync<Customer>("ERP01/SALES/customer001");

// Retrieve collection
var products = await provider.GetDataAsync<List<Product>>("ERP01/CATALOG/products");

// Save data
await provider.SaveDataAsync(customer, "ERP01", "SALES", "customer001");
```

#### Dependency Injection

```csharp
using SAPMock.Data.Extensions;
using SAPMock.Configuration;

// In your startup/configuration
services.AddFileBasedMockDataProvider(configuration);
```

### Layer Priority

When `EnableExtensions` is true:
1. First checks the "extensions" layer
2. Falls back to "common" layer if not found

When `EnableExtensions` is false:
- Only uses the "common" layer

### Key Format

Keys follow the pattern: `{system}/{module}/{key}`

- `system`: The SAP system identifier (e.g., "ERP01", "CRM01")
- `module`: The module within the system (e.g., "SALES", "CATALOG")
- `key`: The specific data key (e.g., "customer001", "products")

### Examples

The `Examples` folder contains `FileBasedMockDataProviderExample.cs` which demonstrates:

- Creating test data
- Single object retrieval
- Collection retrieval
- Layer fallback behavior
- Caching functionality

### Configuration

The provider can be configured through the `SAPMockConfiguration` class:

```csharp
var configuration = new SAPMockConfiguration
{
    DataPath = "data",
    EnableExtensions = true
};
```

### Error Handling

The provider includes comprehensive error handling:

- File not found exceptions for missing data
- JSON deserialization errors
- Directory creation for save operations
- Logging of all operations for debugging

### Performance

- **Caching**: Frequently accessed data is cached in memory
- **Lazy Loading**: Data is only loaded when requested
- **Efficient Fallback**: Layer fallback is optimized to minimize file system access