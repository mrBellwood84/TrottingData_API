using Application.Cache.Interfaces;
using Application.Cache.Services;
using Models.Interfaces;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache for managing race entities.
///     Provides a grouped lookup index to retrieve all races belonging to a specific competition.
/// </summary>
public class RaceCache<T> : IRaceCache<T> where T : IEntity
{
    private readonly GroupedCacheService<T> _competitionIndex = new();
    private readonly CacheService<T> _master = new();

    /// <summary>
    ///     Retrieves a race by its unique identifier.
    /// </summary>
    public Task<T?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves all cached races associated with a specific competition.
    /// </summary>
    public Task<List<T>?> GetByCompetitionAsync(string competitionId)
    {
        return _competitionIndex.GetAsync(competitionId);
    }

    /// <summary>
    ///     Caches or updates a single race in the master storage.
    /// </summary>
    public Task SetAsync(T item)
    {
        return _master.SetAsync(item.Id, item);
    }

    /// <summary>
    ///     Caches a list of races grouped by competition, and registers all items in the master store.
    /// </summary>
    public async Task SetCompetitionAsync(string competitionId, List<T> items)
    {
        foreach (var item in items) await _master.SetAsync(item.Id, item);
        await _competitionIndex.SetAsync(competitionId, items);
    }
}