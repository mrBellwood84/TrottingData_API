using System.Collections.Concurrent;

namespace Application.Cache.Services;

/// <summary>
/// A thread-safe, in-memory cache service for storing and retrieving data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data to be cached.</typeparam>
/// <remarks>
/// This service uses a <see cref="ConcurrentDictionary{TKey,TValue}"/> to ensure thread safety 
/// when accessed by multiple concurrent HTTP requests. The methods return <see cref="Task"/> 
/// to allow for seamless transition to distributed caching (like Redis) in the future.
/// </remarks>
public class CacheService<T>
{
    private readonly ConcurrentDictionary<string, T> _data = [];

    /// <summary>
    /// Retrieves a cached item by its key.
    /// </summary>
    /// <param name="key">The unique identifier for the cached item.</param>
    /// <returns>
    /// A task containing the cached item of type <typeparamref name="T"/> if found; 
    /// otherwise, <see langword="default"/> (null for reference types).
    /// </returns>
    public Task<T?> Get(string key)
    {
        return Task.FromResult(_data.GetValueOrDefault(key));
    }

    /// <summary>
    /// Retrieves all cached items currently stored in the cache.
    /// </summary>
    /// <returns>A task containing a list of all cached items.</returns>
    public Task<List<T>> GetAll()
    {
        return Task.FromResult(_data.Values.ToList());
    }

    /// <summary>
    /// Adds or updates an item in the cache with the specified key.
    /// </summary>
    /// <param name="key">The unique identifier for the item.</param>
    /// <param name="value">The item to be cached.</param>
    /// <returns>A completed task representing the asynchronous operation.</returns>
    public Task Set(string key, T value)
    {
        _data[key] = value;
        return Task.CompletedTask;
    }
}