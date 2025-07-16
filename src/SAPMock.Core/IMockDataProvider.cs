namespace SAPMock.Core;

/// <summary>
/// Provides data storage and retrieval capabilities for mock data.
/// </summary>
public interface IMockDataProvider
{
    /// <summary>
    /// Asynchronously retrieves data of the specified type using the provided key.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The key used to identify the data.</param>
    /// <returns>A task that represents the asynchronous operation, containing the retrieved data.</returns>
    Task<T> GetDataAsync<T>(string key);
    
    /// <summary>
    /// Asynchronously retrieves a collection of data of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of data in the collection.</typeparam>
    /// <returns>A task that represents the asynchronous operation, containing the collection of data.</returns>
    Task<IEnumerable<T>> GetCollectionAsync<T>();
    
    /// <summary>
    /// Asynchronously saves data of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of data to save.</typeparam>
    /// <param name="data">The data to save.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveDataAsync<T>(T data);
}