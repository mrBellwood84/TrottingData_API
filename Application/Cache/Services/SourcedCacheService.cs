using System.Collections.Concurrent;
using Models.Interfaces;

namespace Application.Cache.Services;

/// <summary>
///     A specialized, thread-safe in-memory cache service that keys and indexes entities by their external Source ID.
/// </summary>
/// <typeparam name="T">The entity type to be cached, which must implement <see cref="ISourcedEntity"/>.</typeparam>
public class SourcedCacheService<T> where T : ISourcedEntity
{
    private readonly ConcurrentDictionary<string, T> _data = [];

    /// <summary>
    ///     Retrieves a cached entity by its external Source ID.
    /// </summary>
    /// <param name="key">The external Source ID of the entity.</param>
    /// <returns>The cached entity if found; otherwise, <see langword="null"/>.</returns>
    public Task<T?> Get(string key)
    {
        return Task.FromResult(_data.GetValueOrDefault(key));
    }
    
    /// <summary>
    ///     Adds or updates an entity in the cache using its Source ID as the key.
    /// </summary>
    /// <param name="item">The sourced entity instance to cache.</param>
    public Task Set(T item)
    {
        _data[item.SourceId] = item;
        return Task.CompletedTask;
    }
}