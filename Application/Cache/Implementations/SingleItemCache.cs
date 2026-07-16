using Application.Cache.Interfaces;
using Application.Cache.Services;
using Models.Interfaces;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache implementation for managing single entities.
///     Provides baseline lookup and storage using the entity's primary identifier.
/// </summary>
public class SingleItemCache<T> : ISingleItemCache<T> where T : IEntity
{
    private readonly CacheService<T> _master = new();

    /// <summary>
    ///     Retrieves a cached item by its unique identifier.
    /// </summary>
    public Task<T?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Caches a single item using its primary entity identifier as the key.
    /// </summary>
    public Task SetAsync(T item)
    {
        return _master.SetAsync(item.Id, item);
    }
}