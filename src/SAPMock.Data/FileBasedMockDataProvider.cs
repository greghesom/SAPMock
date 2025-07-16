using System.Collections.Concurrent;
using System.Text.Json;
using SAPMock.Core;

namespace SAPMock.Data;

/// <summary>
/// File-based implementation of IMockDataProvider that loads mock data from JSON files.
/// Follows the path pattern: {DataPath}/{layer}/{system}/{module}/{key}.json
/// </summary>
public class FileBasedMockDataProvider : IMockDataProvider
{
    private readonly string _dataPath;
    private readonly bool _enableExtensions;
    private readonly ConcurrentDictionary<string, object> _cache;
    private readonly JsonSerializerOptions _jsonOptions;
    
    /// <summary>
    /// Initializes a new instance of the FileBasedMockDataProvider.
    /// </summary>
    /// <param name="dataPath">The base path for data files.</param>
    /// <param name="enableExtensions">Whether to check extensions layer first.</param>
    public FileBasedMockDataProvider(string dataPath, bool enableExtensions = true)
    {
        _dataPath = dataPath ?? throw new ArgumentNullException(nameof(dataPath));
        _enableExtensions = enableExtensions;
        _cache = new ConcurrentDictionary<string, object>();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Asynchronously retrieves data of the specified type using the provided key.
    /// Key format: {system}/{module}/{key} or {system}/{module}
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The key used to identify the data.</param>
    /// <returns>A task that represents the asynchronous operation, containing the retrieved data.</returns>
    public async Task<T> GetDataAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        // Check cache first
        var cacheKey = $"{typeof(T).Name}:{key}";
        if (_cache.TryGetValue(cacheKey, out var cachedData))
        {
            return (T)cachedData;
        }

        // Parse key to extract system, module, and data key
        var keyParts = key.Split('/');
        if (keyParts.Length < 2)
            throw new ArgumentException("Key must be in format {system}/{module}/{key} or {system}/{module}", nameof(key));

        var system = keyParts[0];
        var module = keyParts[1];
        var dataKey = keyParts.Length > 2 ? keyParts[2] : "index"; // Default to "index" if no specific key

        // Try to load from extensions first (if enabled), then fall back to common
        var data = await LoadDataFromLayersAsync<T>(system, module, dataKey);
        
        if (data == null)
        {
            throw new FileNotFoundException($"Data not found for key: {key}");
        }

        // Cache the result
        _cache.TryAdd(cacheKey, data);
        
        return data;
    }

    /// <summary>
    /// Asynchronously retrieves a collection of data of the specified type.
    /// Uses the system and module from the type name or a default pattern.
    /// </summary>
    /// <typeparam name="T">The type of data in the collection.</typeparam>
    /// <returns>A task that represents the asynchronous operation, containing the collection of data.</returns>
    public async Task<IEnumerable<T>> GetCollectionAsync<T>()
    {
        // For collections, we'll look for files in the pattern and aggregate them
        // This is a simplified implementation that looks for "collection" files
        var typeName = typeof(T).Name.ToLower();
        var collectionKey = $"default/default/{typeName}";
        
        try
        {
            var data = await GetDataAsync<IEnumerable<T>>(collectionKey);
            return data;
        }
        catch (FileNotFoundException)
        {
            // If no collection file found, return empty collection
            return Enumerable.Empty<T>();
        }
    }

    /// <summary>
    /// Asynchronously saves data of the specified type.
    /// Saves to the extensions layer if enabled, otherwise to common layer.
    /// </summary>
    /// <typeparam name="T">The type of data to save.</typeparam>
    /// <param name="data">The data to save.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveDataAsync<T>(T data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        // Default save location - this would need to be enhanced to accept a key parameter
        var typeName = typeof(T).Name.ToLower();
        var layer = _enableExtensions ? "extensions" : "common";
        var system = "default";
        var module = "default";
        var key = typeName;

        await SaveDataToFileAsync(data, layer, system, module, key);
    }

    /// <summary>
    /// Saves data to a specific file path.
    /// </summary>
    /// <param name="data">The data to save.</param>
    /// <param name="system">The system identifier.</param>
    /// <param name="module">The module identifier.</param>
    /// <param name="key">The data key.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveDataAsync<T>(T data, string system, string module, string key)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        
        if (string.IsNullOrWhiteSpace(system))
            throw new ArgumentException("System cannot be null or empty", nameof(system));
        
        if (string.IsNullOrWhiteSpace(module))
            throw new ArgumentException("Module cannot be null or empty", nameof(module));
        
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        var layer = _enableExtensions ? "extensions" : "common";
        await SaveDataToFileAsync(data, layer, system, module, key);
        
        // Update cache
        var cacheKey = $"{typeof(T).Name}:{system}/{module}/{key}";
        _cache.AddOrUpdate(cacheKey, data, (k, v) => data);
    }

    /// <summary>
    /// Loads data from layers with fallback logic.
    /// </summary>
    private async Task<T?> LoadDataFromLayersAsync<T>(string system, string module, string key)
    {
        var layers = _enableExtensions ? new[] { "extensions", "common" } : new[] { "common" };
        
        foreach (var layer in layers)
        {
            try
            {
                var filePath = BuildFilePath(layer, system, module, key);
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    var data = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                    if (data != null)
                    {
                        LogInfo($"Loaded data from {filePath}");
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading data from {layer}/{system}/{module}/{key}: {ex.Message}");
            }
        }
        
        return default;
    }

    /// <summary>
    /// Saves data to a file with the specified path components.
    /// </summary>
    private async Task SaveDataToFileAsync<T>(T data, string layer, string system, string module, string key)
    {
        var filePath = BuildFilePath(layer, system, module, key);
        var directory = Path.GetDirectoryName(filePath);
        
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
            LogInfo($"Saved data to {filePath}");
        }
        catch (Exception ex)
        {
            LogError($"Error saving data to {filePath}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Builds the file path according to the pattern: {DataPath}/{layer}/{system}/{module}/{key}.json
    /// </summary>
    private string BuildFilePath(string layer, string system, string module, string key)
    {
        return Path.Combine(_dataPath, layer, system, module, $"{key}.json");
    }

    /// <summary>
    /// Clears the cache.
    /// </summary>
    public void ClearCache()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Simple logging method - in a real implementation, this would use a proper logging framework.
    /// </summary>
    private void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    /// <summary>
    /// Simple error logging method - in a real implementation, this would use a proper logging framework.
    /// </summary>
    private void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }
}
