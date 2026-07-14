using System.Collections.Concurrent;
using Models.Interfaces;

namespace Application.Cache.Services;

/// <summary>
///     A specialized, thread-safe in-memory cache service for storing and retrieving external source-mapped entities.
///     Unlike the standard cache, this twin keys and indexes items by their external <see cref="ISourcedEntity.SourceId"/>.
/// </summary>
/// <typeparam name="T">The entity type to be cached, which must implement <see cref="ISourcedEntity"/>.</typeparam>
public class SourcedCacheService<T> where T : ISourcedEntity
{
    /// <summary>
    ///     The thread-safe dictionary holding the cached items, keyed by their external Source ID.
    /// </summary>
    private readonly ConcurrentDictionary<string, T> _data = [];

    /// <summary>
    ///     Retrieves a cached entity by its external source identifier.
    /// </summary>
    /// <param name="key">The external Source ID (e.g., Rikstoto ID, external client key) of the entity.</param>
    /// <returns>
    ///     A task containing the matching entity, or <see langword="null"/> if the entity is not in the cache.
    /// </returns>
    public Task<T?> Get(string key)
    {
        // Task.FromResult satisfies the async signature while performing an instantaneous memory-lookup.
        return Task.FromResult(_data.GetValueOrDefault(key));
    }

    /// <summary>
    ///     Retrieves all currently cached entities as a flat list.
    /// </summary>
    /// <returns>A task containing a list of all cached entities.</returns>
    public Task<List<T>> GetAll()
    {
        // .Values.ToList() creates an isolated point-in-time snapshot of the current cache state.
        return Task.FromResult(_data.Values.ToList());
    }
    
    /// <summary>
    ///     Adds an entity to the cache, or updates it if it already exists.
    ///     Uses the entity's external Source ID as the lookup key.
    /// </summary>
    /// <param name="item">The sourced entity instance to cache.</param>
    /// <returns>A completed task representing the synchronous write operation.</returns>
    public Task Set(T item)
    {
        // EXPLICIT KEYING: Indexes the item by SourceId instead of the internal database Guid.
        _data[item.SourceId] = item;
        return Task.CompletedTask;
    }
}