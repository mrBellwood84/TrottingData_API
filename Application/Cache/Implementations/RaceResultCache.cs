using Application.Cache.Services;
using Models.Complex;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache for managing RaceResultsComplex entities.
///     Provides dual-index 1-to-1 lookups by either the result's primary ID or the associated participant ID.
/// </summary>
public class RaceResultCache : IRaceResultCache
{
    private readonly CacheService<RaceResultsComplex> _master = new();
    private readonly CacheService<RaceResultsComplex> _participantIndex = new();

    /// <summary>
    ///     Retrieves a cached race result by its unique identifier.
    /// </summary>
    public Task<RaceResultsComplex?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves a cached race result by the associated participant identifier.
    /// </summary>
    public Task<RaceResultsComplex?> GetByParticipantAsync(string participantId)
    {
        return _participantIndex.GetAsync(participantId);
    }

    /// <summary>
    ///     Caches a race result in both the master store and the participant index.
    /// </summary>
    public async Task SetAsync(RaceResultsComplex item, string participantId)
    {
        await _master.SetAsync(item.Id, item);
        await _participantIndex.SetAsync(participantId, item);
    }
}