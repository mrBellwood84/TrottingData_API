using Models.Complex;

namespace Application.Cache.Implementations;

public interface IRaceResultCache
{
    /// <summary>
    ///     Retrieves a cached race result by its unique identifier.
    /// </summary>
    Task<RaceResultsComplex?> GetAsync(string key);

    /// <summary>
    ///     Retrieves a cached race result by the associated participant identifier.
    /// </summary>
    Task<RaceResultsComplex?> GetByParticipantAsync(string participantId);

    /// <summary>
    ///     Caches a race result in both the master store and the participant index.
    /// </summary>
    Task SetAsync(RaceResultsComplex item, string participantId);
}