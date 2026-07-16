using Application.Cache.Interfaces;
using Application.Cache.Services;
using Models.Interfaces;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache implementation that manages dual-index lookups.
///     Maps entities by both their primary identifier and their external source identifier
///     to ensure fast O(1) resolutions across different query vectors.
/// </summary>
public class ReadSourceCache<T> : IReadSourceCache<T> where T : ISourcedEntity
{
    private readonly CacheService<T> _master = new();
    private readonly CacheService<T> _source = new();

    /// <summary>
    ///     Retrieves a cached item by its primary identifier.
    /// </summary>
    public Task<T?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves a cached item by its external source identifier.
    /// </summary>
    public Task<T?> GetSourceAsync(string key)
    {
        return _source.GetAsync(key);
    }

    /// <summary>
    ///     Stores the item under both its primary ID and its external source ID.
    ///     Maintains unified in-memory object references across both underlying cache stores.
    /// </summary>
    public async Task SetAsync(T item)
    {
        await _master.SetAsync(item.Id, item);
        await _source.SetAsync(item.SourceId, item);
    }
}