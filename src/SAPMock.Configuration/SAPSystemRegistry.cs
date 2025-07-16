using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using SAPMock.Core;

namespace SAPMock.Configuration;

/// <summary>
/// Provides registration and retrieval capabilities for SAP systems with thread-safe access.
/// </summary>
public class SAPSystemRegistry : ISAPSystemRegistry
{
    private readonly IConfigurationService _configurationService;
    private readonly ILogger<SAPSystemRegistry> _logger;
    private readonly ConcurrentDictionary<string, ISAPSystem> _systems = new();
    private readonly ConcurrentDictionary<string, List<ISAPModule>> _systemModules = new();
    private readonly object _initializationLock = new();
    private bool _initialized = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="SAPSystemRegistry"/> class.
    /// </summary>
    /// <param name="configurationService">The configuration service to load system configurations.</param>
    /// <param name="logger">The logger instance.</param>
    public SAPSystemRegistry(IConfigurationService configurationService, ILogger<SAPSystemRegistry> logger)
    {
        _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Asynchronously registers a new SAP system in the registry.
    /// </summary>
    /// <param name="system">The SAP system to register.</param>
    /// <returns>A task that represents the asynchronous registration operation.</returns>
    public async Task RegisterSystem(ISAPSystem system)
    {
        if (system == null)
            throw new ArgumentNullException(nameof(system));

        if (string.IsNullOrWhiteSpace(system.SystemId))
            throw new ArgumentException("System ID cannot be null or empty.", nameof(system));

        _logger.LogInformation("Registering SAP system: {SystemId} - {Name}", system.SystemId, system.Name);

        _systems.AddOrUpdate(system.SystemId, system, (key, existingSystem) => system);

        // Load modules for the newly registered system
        await LoadModulesForSystem(system.SystemId);

        _logger.LogInformation("Successfully registered SAP system: {SystemId}", system.SystemId);
    }

    /// <summary>
    /// Asynchronously retrieves a SAP system by its identifier.
    /// </summary>
    /// <param name="systemId">The unique identifier of the SAP system to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing the SAP system if found.</returns>
    public async Task<ISAPSystem?> GetSystem(string systemId)
    {
        if (string.IsNullOrWhiteSpace(systemId))
            throw new ArgumentException("System ID cannot be null or empty.", nameof(systemId));

        await EnsureInitialized();

        _systems.TryGetValue(systemId, out var system);
        return system;
    }

    /// <summary>
    /// Asynchronously retrieves all registered SAP systems.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing all registered SAP systems.</returns>
    public async Task<IEnumerable<ISAPSystem>> GetAllSystemsAsync()
    {
        await EnsureInitialized();
        return _systems.Values.ToList();
    }

    /// <summary>
    /// Asynchronously retrieves all modules for a specific SAP system.
    /// </summary>
    /// <param name="systemId">The unique identifier of the SAP system.</param>
    /// <returns>A task that represents the asynchronous operation, containing the modules for the specified system.</returns>
    public async Task<IEnumerable<ISAPModule>> GetModulesForSystem(string systemId)
    {
        if (string.IsNullOrWhiteSpace(systemId))
            throw new ArgumentException("System ID cannot be null or empty.", nameof(systemId));

        await EnsureInitialized();

        _systemModules.TryGetValue(systemId, out var modules);
        return modules ?? Enumerable.Empty<ISAPModule>();
    }

    /// <summary>
    /// Performs a health check on a specific SAP system.
    /// </summary>
    /// <param name="systemId">The unique identifier of the SAP system to check.</param>
    /// <returns>A task that represents the asynchronous health check operation, returning true if the system is healthy.</returns>
    public async Task<bool> IsSystemHealthy(string systemId)
    {
        if (string.IsNullOrWhiteSpace(systemId))
            throw new ArgumentException("System ID cannot be null or empty.", nameof(systemId));

        await EnsureInitialized();

        // Check if system exists
        if (!_systems.ContainsKey(systemId))
        {
            _logger.LogWarning("Health check failed: System {SystemId} not found", systemId);
            return false;
        }

        // Check if system has modules
        if (!_systemModules.ContainsKey(systemId) || !_systemModules[systemId].Any())
        {
            _logger.LogWarning("Health check failed: System {SystemId} has no modules", systemId);
            return false;
        }

        _logger.LogDebug("Health check passed for system: {SystemId}", systemId);
        return true;
    }

    /// <summary>
    /// Performs a health check on all registered SAP systems.
    /// </summary>
    /// <returns>A task that represents the asynchronous health check operation, returning a dictionary of system health status.</returns>
    public async Task<Dictionary<string, bool>> GetSystemHealthStatus()
    {
        await EnsureInitialized();

        var healthStatus = new Dictionary<string, bool>();

        foreach (var systemId in _systems.Keys)
        {
            healthStatus[systemId] = await IsSystemHealthy(systemId);
        }

        return healthStatus;
    }

    /// <summary>
    /// Ensures that the registry is initialized with all system configurations.
    /// </summary>
    private Task EnsureInitialized()
    {
        if (_initialized)
            return Task.CompletedTask;

        lock (_initializationLock)
        {
            if (_initialized)
                return Task.CompletedTask;

            // Use synchronous initialization to avoid async lock issues
            InitializeAsync().GetAwaiter().GetResult();
            _initialized = true;
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Loads all system configurations from the configuration service.
    /// </summary>
    private async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Initializing SAP System Registry...");

            // Validate configuration first
            var isValid = await _configurationService.ValidateConfigurationAsync();
            if (!isValid)
            {
                _logger.LogError("Configuration validation failed. Registry initialization aborted.");
                return;
            }

            // Load all system configurations
            var systems = await _configurationService.LoadSystemConfigurationAsync();
            
            foreach (var system in systems)
            {
                _systems.TryAdd(system.SystemId, system);
                await LoadModulesForSystem(system.SystemId);
            }

            _logger.LogInformation("Successfully initialized SAP System Registry with {Count} systems", _systems.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing SAP System Registry");
            throw;
        }
    }

    /// <summary>
    /// Loads modules for a specific SAP system.
    /// </summary>
    /// <param name="systemId">The unique identifier of the SAP system.</param>
    private async Task LoadModulesForSystem(string systemId)
    {
        try
        {
            _logger.LogDebug("Loading modules for system: {SystemId}", systemId);

            var modules = await _configurationService.LoadModuleConfigurationsAsync(systemId);
            var moduleList = modules.ToList();

            _systemModules.AddOrUpdate(systemId, moduleList, (key, existingModules) => moduleList);

            _logger.LogDebug("Loaded {Count} modules for system: {SystemId}", moduleList.Count, systemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading modules for system: {SystemId}", systemId);
            throw;
        }
    }
}