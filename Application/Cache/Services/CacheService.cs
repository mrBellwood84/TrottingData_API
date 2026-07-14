using System.Collections.Concurrent;
using Models.Interfaces;

namespace Application.Cache.Services;

/// <summary>
///     A generic, thread-safe in-memory cache service for storing and retrieving entities.
/// </summary>
/// <typeparam name="T">The entity type to be cached, which must implement <see cref="IEntity"/>.</typeparam>
public class CacheService<T> where T : IEntity
{
    private readonly ConcurrentDictionary<string, T> _data = [];

    /// <summary>
    ///     Retrieves a cached entity by its unique identifier.
    /// </summary>
    /// <param name="key">The unique ID of the entity.</param>
    /// <returns>The cached entity if found; otherwise, <see langword="null"/>.</returns>
    public Task<T?> Get(string key)
    {
        // Return completed task directly to satisfy the async signature
        return Task.FromResult(_data.GetValueOrDefault(key));
    }

    /// <summary>
    ///     Retrieves a point-in-time snapshot of all cached entities.
    /// </summary>
    /// <returns>A list of all currently cached entities.</returns>
    public Task<List<T>> GetAll()
    {
        return Task.FromResult(_data.Values.ToList());
    }
    
    /// <summary>
    ///     Adds or updates an entity in the cache using its ID as the key.
    /// </summary>
    /// <param name="item">The entity instance to cache.</param>
    public Task Set(T item)
    {
        _data[item.Id] = item;
        return Task.CompletedTask;
    }
}