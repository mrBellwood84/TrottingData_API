using System.Collections.Concurrent;

namespace Application.Cache.Services;

/// <summary>
///     A generic, thread-safe in-memory storage service.
///     Acts as the foundational low-level cache engine used by specialized cache implementations.
/// </summary>
public class CacheService<T>
{
    private readonly ConcurrentDictionary<string, T> _data = [];

    /// <summary>
    ///     Retrieves a cached item by its unique key.
    ///     Returns default (null) if the key is not found.
    /// </summary>
    public Task<T?> GetAsync(string key)
    {
        return Task.FromResult(_data.GetValueOrDefault(key));
    }

    /// <summary>
    ///     Retrieves a copy of all currently cached items as a list.
    /// </summary>
    public Task<List<T>> GetAllAsync()
    {
        return Task.FromResult(_data.Values.ToList());
    }

    /// <summary>
    ///     Stores or updates an item in the cache under the specified key.
    /// </summary>
    public Task SetAsync(string key, T item)
    {
        _data[key] = item;
        return Task.CompletedTask;
    }
}