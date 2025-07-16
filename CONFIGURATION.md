# SAP Mock Service Configuration Guide

This guide explains how to configure the SAP Mock Service for different environments and data sets.

## Configuration Overview

The SAP Mock Service uses a layered configuration approach:

1. **Default Configuration** - Base settings in `appsettings.json`
2. **Environment-specific Configuration** - Environment overrides in `appsettings.{Environment}.json`
3. **Command-line Arguments** - Runtime overrides via command line
4. **Environment Variables** - Runtime overrides via environment variables

## Configuration Settings

### Core Configuration Properties

| Property | Default | Description |
|----------|---------|-------------|
| `SAPMock:DataPath` | `../../data` | Path to the mock data directory |
| `SAPMock:ConfigPath` | `../../config` | Path to the configuration files |
| `SAPMock:EnableExtensions` | `true` | Enable extension data loading |
| `SAPMock:ActiveProfile` | `default` | Active configuration profile |

## Data Organization

### Directory Structure

```
data/
├── common/                    # Shared mock data
│   └── ERP01/
│       └── MM/
│           └── materials.json
├── profiles/                  # Profile-specific data
│   ├── development/
│   │   └── ERP01/
│   │       └── MM/
│   │           └── materials.json
│   └── test/
│       └── ERP01/
│           └── MM/
│               └── materials.json
└── extensions/                # Developer-specific extensions
    └── ERP01/
        └── MM/
            └── materials.json
```

### Data Loading Priority

1. **Extensions** (if enabled) - Developer-specific overrides
2. **Profiles** - Environment-specific data
3. **Common** - Shared baseline data

## Running with Different Configurations

### 1. Using Launch Profiles (Visual Studio/IDE)

The API project includes several launch profiles:

- **Development** - Uses development profile data
- **Test** - Uses test profile data with extensions disabled
- **CustomPath** - Demonstrates command-line argument override

### 2. Using Command Line Arguments

```bash
# Use test profile data
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=../../data/profiles/test --SAPMock:ActiveProfile=test

# Use development profile with extensions disabled
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=../../data/profiles/development --SAPMock:EnableExtensions=false

# Use custom data path
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=/path/to/custom/data --SAPMock:ActiveProfile=custom
```

### 3. Using Environment Variables

```bash
# Windows
set SAPMock__DataPath=../../data/profiles/test
set SAPMock__ActiveProfile=test
dotnet run --project src/SAPMock.Api

# Linux/Mac
export SAPMock__DataPath=../../data/profiles/test
export SAPMock__ActiveProfile=test
dotnet run --project src/SAPMock.Api
```

### 4. Using .NET Aspire AppHost

The AppHost project orchestrates the SAP Mock service with proper configuration:

```bash
# Run with default configuration
dotnet run --project src/SAPMock.AppHost

# Run with development profile
dotnet run --project src/SAPMock.AppHost --launch-profile Development

# Run with test profile
dotnet run --project src/SAPMock.AppHost --launch-profile Test

# Run with custom arguments
dotnet run --project src/SAPMock.AppHost -- --SAPMock:DataPath=../../data/profiles/test --SAPMock:ActiveProfile=custom
```

## Configuration Examples

### Development Environment

```json
{
  "SAPMock": {
    "DataPath": "../../data/profiles/development",
    "ConfigPath": "../../config",
    "EnableExtensions": true,
    "ActiveProfile": "development"
  }
}
```

### Test Environment

```json
{
  "SAPMock": {
    "DataPath": "../../data/profiles/test",
    "ConfigPath": "../../config",
    "EnableExtensions": false,
    "ActiveProfile": "test"
  }
}
```

### Integration Testing

```json
{
  "SAPMock": {
    "DataPath": "../../data/profiles/integration",
    "ConfigPath": "../../config",
    "EnableExtensions": false,
    "ActiveProfile": "integration"
  }
}
```

## Creating Custom Data Sets

### 1. Create a New Profile Directory

```bash
mkdir -p data/profiles/your-profile/ERP01/MM
```

### 2. Add Your Mock Data

```json
# data/profiles/your-profile/ERP01/MM/materials.json
[
  {
    "materialNumber": "YOUR-MATERIAL-001",
    "description": "Your Custom Material",
    "materialType": "FERT",
    "baseUnit": "EA",
    "materialGroup": "CUSTOM"
  }
]
```

### 3. Use the Custom Profile

```bash
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=../../data/profiles/your-profile --SAPMock:ActiveProfile=your-profile
```

## Extension Data

Extension data allows developers to override specific mock data without modifying the shared data:

### 1. Create Extension Directory

```bash
mkdir -p data/extensions/ERP01/MM
```

### 2. Add Override Data

```json
# data/extensions/ERP01/MM/materials.json
[
  {
    "materialNumber": "MATERIAL-001",
    "description": "Developer Override Material",
    "materialType": "FERT",
    "baseUnit": "EA",
    "materialGroup": "DEV"
  }
]
```

### 3. Enable Extensions

Extensions are enabled by default. To disable:

```bash
dotnet run --project src/SAPMock.Api -- --SAPMock:EnableExtensions=false
```

## Port Configuration

### Default Ports

- **SAP Mock API**: 5204 (HTTP), 7000 (HTTPS)
- **Example Service**: 5205 (HTTP)
- **Aspire Dashboard**: 15005 (HTTP)

### Custom Port Configuration

Modify the `WithHttpEndpoint` calls in `AppHost.cs`:

```csharp
var sapMockService = builder.AddProject("sap-mock", "../SAPMock.Api/SAPMock.Api.csproj")
    .WithHttpEndpoint(port: 8080, name: "sap-http")
    .WithHttpsEndpoint(port: 8443, name: "sap-https");
```

## Troubleshooting

### Common Issues

1. **Data Not Loading**
   - Check that the `DataPath` points to the correct directory
   - Verify that the JSON files are valid
   - Ensure the directory structure matches the expected format

2. **Configuration Not Applied**
   - Verify command-line argument syntax (double dash `--`)
   - Check environment variable naming (double underscore `__`)
   - Ensure configuration precedence is correct

3. **Port Conflicts**
   - Change the port numbers in the configuration
   - Check for other services using the same ports

### Debug Configuration

Enable debug logging to see configuration values:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "SAPMock": "Debug"
    }
  }
}
```

## Best Practices

1. **Version Control**: Include common data and profiles in version control, exclude extensions
2. **Environment Separation**: Use different profiles for different environments
3. **Data Validation**: Validate JSON files before deployment
4. **Documentation**: Document custom data structures and profiles
5. **Testing**: Test with different configurations before deployment

## Example: Switching Between Data Sets

### Scenario 1: Development with Custom Materials

```bash
# Create development profile with custom materials
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=../../data/profiles/development --SAPMock:ActiveProfile=development
```

### Scenario 2: Integration Testing with Minimal Data

```bash
# Use test profile with extensions disabled
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=../../data/profiles/test --SAPMock:EnableExtensions=false --SAPMock:ActiveProfile=test
```

### Scenario 3: Demo with Rich Data Set

```bash
# Use common data with extensions enabled
dotnet run --project src/SAPMock.Api -- --SAPMock:DataPath=../../data/common --SAPMock:EnableExtensions=true --SAPMock:ActiveProfile=demo
```