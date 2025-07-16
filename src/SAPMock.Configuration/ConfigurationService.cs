using System.Text.Json;
using SAPMock.Core;

namespace SAPMock.Configuration;

/// <summary>
/// Provides configuration management capabilities for SAP systems and modules.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly SAPMockConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;

    public ConfigurationService(SAPMockConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Asynchronously loads system configuration from JSON files.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the configuration.</returns>
    public async Task<IEnumerable<ISAPSystem>> LoadSystemConfigurationAsync()
    {
        var systems = new List<ISAPSystem>();
        
        // Apply environment variable overrides to configuration
        ApplyEnvironmentVariableOverrides();
        
        // Load systems from configuration
        foreach (var systemConfig in _configuration.Systems.Where(s => s.Enabled))
        {
            var system = new SAPSystem
            {
                SystemId = systemConfig.SystemId,
                Name = systemConfig.Name,
                Type = systemConfig.Type,
                ConnectionParameters = new Dictionary<string, string>(systemConfig.ConnectionParameters)
            };
            
            systems.Add(system);
        }
        
        // Load additional systems from JSON files if config path exists
        if (Directory.Exists(_configuration.ConfigPath))
        {
            var systemFiles = Directory.GetFiles(_configuration.ConfigPath, "system-*.json");
            
            foreach (var systemFile in systemFiles)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(systemFile);
                    var systemConfig = JsonSerializer.Deserialize<SAPSystemConfig>(json, _jsonOptions);
                    
                    if (systemConfig != null && systemConfig.Enabled)
                    {
                        var system = new SAPSystem
                        {
                            SystemId = systemConfig.SystemId,
                            Name = systemConfig.Name,
                            Type = systemConfig.Type,
                            ConnectionParameters = new Dictionary<string, string>(systemConfig.ConnectionParameters)
                        };
                        
                        systems.Add(system);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue processing other files
                    Console.WriteLine($"Error loading system configuration from {systemFile}: {ex.Message}");
                }
            }
        }
        
        return systems;
    }

    /// <summary>
    /// Asynchronously loads module configurations and filters by supported systems.
    /// </summary>
    /// <param name="systemId">The system identifier to filter modules by.</param>
    /// <returns>A task that represents the asynchronous operation, containing the modules for the specified system.</returns>
    public async Task<IEnumerable<ISAPModule>> LoadModuleConfigurationsAsync(string systemId)
    {
        var modules = new List<ISAPModule>();
        
        // Load modules from configuration
        var systemConfig = _configuration.Systems.FirstOrDefault(s => s.SystemId == systemId && s.Enabled);
        if (systemConfig != null)
        {
            foreach (var moduleConfig in systemConfig.Modules.Where(m => m.Enabled))
            {
                var endpoints = moduleConfig.Endpoints.Where(e => e.Enabled).Select(e => new SAPEndpoint
                {
                    Path = e.Path,
                    Method = e.Method,
                    RequestType = typeof(object), // This would need to be resolved from type name
                    ResponseType = typeof(object), // This would need to be resolved from type name
                    Handler = async (request) => await Task.FromResult<object>(new { }) // Default handler
                });
                
                var module = new SAPModule
                {
                    ModuleId = moduleConfig.ModuleId,
                    Name = moduleConfig.Name,
                    SystemId = moduleConfig.SystemId,
                    Endpoints = endpoints
                };
                
                modules.Add(module);
            }
        }
        
        // Load additional modules from JSON files if config path exists
        if (Directory.Exists(_configuration.ConfigPath))
        {
            var moduleFiles = Directory.GetFiles(_configuration.ConfigPath, $"module-{systemId}-*.json");
            
            foreach (var moduleFile in moduleFiles)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(moduleFile);
                    var moduleConfig = JsonSerializer.Deserialize<ModuleConfig>(json, _jsonOptions);
                    
                    if (moduleConfig != null && moduleConfig.Enabled && moduleConfig.SystemId == systemId)
                    {
                        var endpoints = moduleConfig.Endpoints.Where(e => e.Enabled).Select(e => new SAPEndpoint
                        {
                            Path = e.Path,
                            Method = e.Method,
                            RequestType = typeof(object), // This would need to be resolved from type name
                            ResponseType = typeof(object), // This would need to be resolved from type name
                            Handler = async (request) => await Task.FromResult<object>(new { }) // Default handler
                        });
                        
                        var module = new SAPModule
                        {
                            ModuleId = moduleConfig.ModuleId,
                            Name = moduleConfig.Name,
                            SystemId = moduleConfig.SystemId,
                            Endpoints = endpoints
                        };
                        
                        modules.Add(module);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue processing other files
                    Console.WriteLine($"Error loading module configuration from {moduleFile}: {ex.Message}");
                }
            }
        }
        
        return modules;
    }

    /// <summary>
    /// Validates the configuration for completeness and correctness.
    /// </summary>
    /// <returns>A task that represents the asynchronous validation operation, returning true if valid.</returns>
    public async Task<bool> ValidateConfigurationAsync()
    {
        await Task.CompletedTask; // Make method async
        
        // Validate basic configuration
        if (string.IsNullOrWhiteSpace(_configuration.DataPath))
            return false;
            
        if (string.IsNullOrWhiteSpace(_configuration.ConfigPath))
            return false;
            
        if (string.IsNullOrWhiteSpace(_configuration.ActiveProfile))
            return false;
        
        // Validate systems
        foreach (var system in _configuration.Systems)
        {
            if (string.IsNullOrWhiteSpace(system.SystemId))
                return false;
                
            if (string.IsNullOrWhiteSpace(system.Name))
                return false;
                
            if (string.IsNullOrWhiteSpace(system.Type))
                return false;
            
            // Validate modules
            foreach (var module in system.Modules)
            {
                if (string.IsNullOrWhiteSpace(module.ModuleId))
                    return false;
                    
                if (string.IsNullOrWhiteSpace(module.Name))
                    return false;
                    
                if (module.SystemId != system.SystemId)
                    return false;
                
                // Validate endpoints
                foreach (var endpoint in module.Endpoints)
                {
                    if (string.IsNullOrWhiteSpace(endpoint.Path))
                        return false;
                        
                    if (string.IsNullOrWhiteSpace(endpoint.Method))
                        return false;
                }
            }
        }
        
        return true;
    }

    private void ApplyEnvironmentVariableOverrides()
    {
        // Support environment variable overrides
        var dataPath = Environment.GetEnvironmentVariable("SAPMOCK_DATA_PATH");
        if (!string.IsNullOrWhiteSpace(dataPath))
            _configuration.DataPath = dataPath;
            
        var configPath = Environment.GetEnvironmentVariable("SAPMOCK_CONFIG_PATH");
        if (!string.IsNullOrWhiteSpace(configPath))
            _configuration.ConfigPath = configPath;
            
        var enableExtensions = Environment.GetEnvironmentVariable("SAPMOCK_ENABLE_EXTENSIONS");
        if (!string.IsNullOrWhiteSpace(enableExtensions) && bool.TryParse(enableExtensions, out bool extensions))
            _configuration.EnableExtensions = extensions;
            
        var activeProfile = Environment.GetEnvironmentVariable("SAPMOCK_ACTIVE_PROFILE");
        if (!string.IsNullOrWhiteSpace(activeProfile))
            _configuration.ActiveProfile = activeProfile;
    }
}