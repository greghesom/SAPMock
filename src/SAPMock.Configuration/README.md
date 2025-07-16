# SAP Mock Configuration Management

This document describes the configuration management system for SAP Mock.

## Overview

The configuration management system provides a flexible way to configure SAP systems, modules, and endpoints. It supports both programmatic configuration and JSON file-based configuration with environment variable overrides.

## Key Components

### Configuration Classes

- **SAPMockConfiguration**: Main configuration class containing:
  - `DataPath`: Path to the data directory
  - `ConfigPath`: Path to the configuration directory
  - `EnableExtensions`: Whether extensions are enabled
  - `ActiveProfile`: Active profile name
  - `Systems`: List of SAP system configurations

- **SAPSystemConfig**: SAP system configuration containing:
  - `SystemId`: Unique system identifier
  - `Name`: Display name
  - `Type`: System type (ERP, CRM, etc.)
  - `ConnectionParameters`: Connection parameters
  - `Modules`: Available modules
  - `Enabled`: Whether the system is enabled

- **ModuleConfig**: Module configuration containing:
  - `ModuleId`: Unique module identifier
  - `Name`: Display name
  - `SystemId`: Parent system identifier
  - `Endpoints`: Available endpoints
  - `Enabled`: Whether the module is enabled
  - `ConfigFile`: Configuration file path

- **EndpointConfig**: Endpoint configuration containing:
  - `Path`: Endpoint path
  - `Method`: HTTP method
  - `RequestType`: Request type name
  - `ResponseType`: Response type name
  - `HandlerType`: Handler type name
  - `Enabled`: Whether the endpoint is enabled

### Configuration Service

The `ConfigurationService` class implements `IConfigurationService` and provides:

- **LoadSystemConfigurationAsync()**: Loads system configurations from programmatic configuration and JSON files
- **LoadModuleConfigurationsAsync(systemId)**: Loads module configurations for a specific system
- **ValidateConfigurationAsync()**: Validates configuration completeness and correctness

## Configuration Files

### System Configuration Files

System configurations are stored in JSON files following the pattern `system-{SystemId}.json`:

```json
{
  "systemId": "ERP01",
  "name": "SAP ERP Development",
  "type": "ERP",
  "enabled": true,
  "connectionParameters": {
    "server": "sap-erp-dev.company.com",
    "port": "8000",
    "client": "100",
    "language": "EN"
  },
  "modules": [
    {
      "moduleId": "FI",
      "name": "Financial Accounting",
      "systemId": "ERP01",
      "enabled": true,
      "endpoints": [
        {
          "path": "/sap/opu/rest/fi/general-ledger",
          "method": "GET",
          "requestType": "object",
          "responseType": "object",
          "handlerType": "DefaultHandler",
          "enabled": true
        }
      ]
    }
  ]
}
```

### Module Configuration Files

Module configurations are stored in JSON files following the pattern `module-{SystemId}-{ModuleId}.json`:

```json
{
  "moduleId": "MM",
  "name": "Materials Management",
  "systemId": "ERP01",
  "enabled": true,
  "endpoints": [
    {
      "path": "/sap/opu/rest/mm/materials",
      "method": "GET",
      "requestType": "object",
      "responseType": "object",
      "handlerType": "MaterialsHandler",
      "enabled": true
    }
  ]
}
```

## Environment Variable Overrides

The following environment variables can be used to override configuration settings:

- `SAPMOCK_DATA_PATH`: Override the data path
- `SAPMOCK_CONFIG_PATH`: Override the configuration path
- `SAPMOCK_ENABLE_EXTENSIONS`: Override the enable extensions flag
- `SAPMOCK_ACTIVE_PROFILE`: Override the active profile

## Usage Example

```csharp
// Create configuration
var configuration = new SAPMockConfiguration
{
    DataPath = "data",
    ConfigPath = "config",
    EnableExtensions = true,
    ActiveProfile = "development"
};

// Create configuration service
var configurationService = new ConfigurationService(configuration);

// Validate configuration
bool isValid = await configurationService.ValidateConfigurationAsync();

// Load system configurations
var systems = await configurationService.LoadSystemConfigurationAsync();

// Load module configurations for a specific system
var modules = await configurationService.LoadModuleConfigurationsAsync("ERP01");
```

## Error Handling

The configuration service includes error handling for:
- Invalid JSON files
- Missing configuration files
- Invalid configuration data
- Environment variable parsing errors

Errors are logged to the console but don't prevent the loading of other valid configurations.