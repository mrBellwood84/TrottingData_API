using System.Collections.Concurrent;
using Models.Interfaces;

namespace Application.Cache.Services;

/// <summary>
///     A generic, thread-safe in-memory cache service for storing and retrieving entities.
/// </summary>
/// <typeparam name="T">The entity type to be cached, which must implement <see cref="IEntity"/>.</typeparam>
public class CacheService<T> where T : IEntity
{
    /// <summary>
    ///     The thread-safe dictionary holding the cached items, keyed by their unique identifier.
    /// </summary>
    private readonly ConcurrentDictionary<string, T> _data = [];

    /// <summary>
    ///     Retrieves a cached entity by its unique database identifier.
    /// </summary>
    /// <param name="key">The unique ID of the entity to retrieve.</param>
    /// <returns>
    ///     A task containing the matching entity, or <see langword="null"/> if the entity is not in the cache.
    /// </returns>
    public Task<T?> Get(string key)
    {
        // Task.FromResult is used to satisfy the asynchronous signature 
        // while performing an instantaneous in-memory lookup.
        return Task.FromResult(_data.GetValueOrDefault(key));
    }

    /// <summary>
    ///     Retrieves all currently cached entities as a flat list.
    /// </summary>
    /// <returns>A task containing a list of all cached entities.</returns>
    public Task<List<T>> GetAll()
    {
        // .Values.ToList() creates a point-in-time snapshot of the dictionary values.
        return Task.FromResult(_data.Values.ToList());
    }
    
    /// <summary>
    ///     Adds an entity to the cache, or updates it if it already exists.
    ///     Uses the entity's internal ID as the lookup key.
    /// </summary>
    /// <param name="item">The entity instance to cache.</param>
    /// <returns>A completed task representing the synchronous write operation.</returns>
    public Task Set(T item)
    {
        // ConcurrentDictionary indexer behaves as an "Add or Update" operation.
        _data[item.Id] = item;
        return Task.CompletedTask;
    }
}