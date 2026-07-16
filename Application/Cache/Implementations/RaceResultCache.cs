using Application.Cache.Interfaces;
using Application.Cache.Services;
using Models.Interfaces;

namespace Application.Cache.Implementations;

/// <summary>
///     A generic in-memory cache providing dual-index 1-to-1 lookups for any entity implementing <see cref="IEntity" />.
///     Maintains synchronous mapping between primary IDs and associated participant IDs.
/// </summary>
/// <typeparam name="T">The model type to be cached, which must implement <see cref="IEntity" />.</typeparam>
public class RaceResultCache<T> : IRaceResultCache<T> where T : IEntity
{
    private readonly CacheService<T> _master = new();
    private readonly CacheService<T> _participantIndex = new();

    /// <summary>
    ///     Retrieves a cached item by its primary identifier.
    /// </summary>
    /// <param name="key">The primary ID of the cached item.</param>
    public Task<T?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves a cached item by its associated participant identifier.
    /// </summary>
    /// <param name="participantId">The participant ID used as the secondary lookup index.</param>
    public Task<T?> GetByParticipantAsync(string participantId)
    {
        return _participantIndex.GetAsync(participantId);
    }

    /// <summary>
    ///     Stores an item in both the primary master cache and the secondary participant index.
    /// </summary>
    /// <param name="participantId">The participant ID to map against the cached item.</param>
    /// <param name="item">The item instance to store.</param>
    public async Task SetAsync(string participantId, T? item)
    {
        if (item is null) return;

        await _master.SetAsync(item.Id, item);
        await _participantIndex.SetAsync(participantId, item);
    }
}