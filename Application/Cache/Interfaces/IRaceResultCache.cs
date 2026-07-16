using Models.Interfaces;

namespace Application.Cache.Interfaces;

public interface IRaceResultCache<T> where T : IEntity
{
    /// <summary>
    ///     Retrieves a cached race result by its unique identifier.
    /// </summary>
    Task<T?> GetAsync(string key);

    /// <summary>
    ///     Retrieves a cached race result by the associated participant identifier.
    /// </summary>
    Task<T?> GetByParticipantAsync(string participantId);

    /// <summary>
    ///     Caches a race result in both the master store and the participant index.
    /// </summary>
    Task SetAsync(string participantId, T? item);
}