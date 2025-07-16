using SAPMock.Configuration;
using SAPMock.Core;

namespace SAPMock.Configuration.Examples;

/// <summary>
/// Example showing how to use the configuration service.
/// </summary>
public static class ConfigurationExample
{
    /// <summary>
    /// Demonstrates how to create and use the configuration service.
    /// </summary>
    public static async Task RunExample()
    {
        // Create configuration
        var configuration = new SAPMockConfiguration
        {
            DataPath = "data",
            ConfigPath = "config",
            EnableExtensions = true,
            ActiveProfile = "development",
            Systems = new List<SAPSystemConfig>
            {
                new SAPSystemConfig
                {
                    SystemId = "CRM01",
                    Name = "SAP CRM Production",
                    Type = "CRM",
                    Enabled = true,
                    ConnectionParameters = new Dictionary<string, string>
                    {
                        { "server", "sap-crm-prod.company.com" },
                        { "port", "8080" },
                        { "client", "100" }
                    },
                    Modules = new List<ModuleConfig>
                    {
                        new ModuleConfig
                        {
                            ModuleId = "SALES",
                            Name = "Sales Management",
                            SystemId = "CRM01",
                            Enabled = true,
                            Endpoints = new List<EndpointConfig>
                            {
                                new EndpointConfig
                                {
                                    Path = "/sap/opu/rest/crm/sales",
                                    Method = "GET",
                                    RequestType = "object",
                                    ResponseType = "object",
                                    HandlerType = "SalesHandler",
                                    Enabled = true
                                }
                            }
                        }
                    }
                }
            }
        };

        // Create configuration service
        var configurationService = new ConfigurationService(configuration, null!);

        // Validate configuration
        bool isValid = await configurationService.ValidateConfigurationAsync();
        Console.WriteLine($"Configuration is valid: {isValid}");

        // Load system configurations
        var systems = await configurationService.LoadSystemConfigurationAsync();
        Console.WriteLine($"Loaded {systems.Count()} systems:");
        foreach (var system in systems)
        {
            Console.WriteLine($"  - {system.SystemId}: {system.Name} ({system.Type})");
        }

        // Load module configurations for a specific system
        var modules = await configurationService.LoadModuleConfigurationsAsync("ERP01");
        Console.WriteLine($"Loaded {modules.Count()} modules for ERP01:");
        foreach (var module in modules)
        {
            Console.WriteLine($"  - {module.ModuleId}: {module.Name}");
            Console.WriteLine($"    Endpoints: {module.Endpoints.Count()}");
        }
    }
}