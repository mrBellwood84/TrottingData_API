using Application.Cache.Interfaces;
using Application.Cache.Services;
using Models.Interfaces;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache implementation optimized for handling collections of entities.
///     Tracks initialization state via a loaded flag to support conditional bulk operations.
/// </summary>
public class ReadAllCache<T> : IReadAllCache<T> where T : IEntity
{
    private readonly CacheService<T> _master = new();

    /// <summary>
    ///     Gets a value indicating whether the full dataset has been loaded into the cache.
    /// </summary>
    public bool Loaded { get; private set; }

    /// <summary>
    ///     Retrieves a cached item by its unique identifier.
    /// </summary>
    public Task<T?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves all items currently stored within this cache instance.
    /// </summary>
    public Task<List<T>> GetAllAsync()
    {
        return _master.GetAllAsync();
    }

    /// <summary>
    ///     Caches a single item using its primary entity identifier as the key.
    /// </summary>
    public Task SetAsync(T item)
    {
        return _master.SetAsync(item.Id, item);
    }

    /// <summary>
    ///     Populates the cache with a list of items sequentially and marks the collection as fully loaded.
    /// </summary>
    public async Task SetAsync(List<T> items)
    {
        foreach (var item in items)
            await _master.SetAsync(item.Id, item);

        Loaded = true;
    }
}