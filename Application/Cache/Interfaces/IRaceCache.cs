using Models.Interfaces;

namespace Application.Cache.Implementations;

public interface IRaceCache<T> where T : IEntity
{
    /// <summary>
    ///     Retrieves a race by its unique identifier.
    /// </summary>
    Task<T?> GetAsync(string key);

    /// <summary>
    ///     Retrieves all cached races associated with a specific competition.
    /// </summary>
    Task<List<T>?> GetByCompetitionAsync(string competitionId);

    /// <summary>
    ///     Caches or updates a single race in the master storage.
    /// </summary>
    Task SetAsync(T item);

    /// <summary>
    ///     Caches a list of races grouped by competition, and registers all items in the master store.
    /// </summary>
    Task SetCompetitionAsync(string competitionId, List<T> items);
}