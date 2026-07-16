using Application.Cache.Services;
using Models.Complex;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache for managing RaceParticipantComplex entities.
///     Provides isolated indexing paths for races, drivers, horses, and trainers
///     to prevent data contamination across different query vectors.
/// </summary>
public class RaceParticipantCache : IRaceParticipantCache
{
    private readonly CacheService<RaceParticipantComplex> _master = new();
    private readonly GroupedCacheService<RaceParticipantComplex> _raceIndex = new();
    private readonly GroupedCacheService<RaceParticipantComplex> _driverIndex = new();
    private readonly GroupedCacheService<RaceParticipantComplex> _horseIndex = new();
    private readonly GroupedCacheService<RaceParticipantComplex> _trainerIndex = new();

    /// <summary>
    ///     Retrieves a participant by their unique identifier.
    /// </summary>
    public Task<RaceParticipantComplex?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves all cached participants for a specific race.
    /// </summary>
    public Task<List<RaceParticipantComplex>?> GetByRaceAsync(string raceId)
    {
        return _raceIndex.GetAsync(raceId);
    }

    /// <summary>
    ///     Retrieves all cached race participations for a specific driver.
    /// </summary>
    public Task<List<RaceParticipantComplex>?> GetByDriverAsync(string driverId)
    {
        return _driverIndex.GetAsync(driverId);
    }

    /// <summary>
    ///     Retrieves all cached race participations for a specific horse.
    /// </summary>
    public Task<List<RaceParticipantComplex>?> GetByHorseAsync(string horseId)
    {
        return _horseIndex.GetAsync(horseId);
    }

    /// <summary>
    ///     Retrieves all cached race participations for a specific trainer.
    /// </summary>
    public Task<List<RaceParticipantComplex>?> GetByTrainAsync(string trainerId)
    {
        return _trainerIndex.GetAsync(trainerId);
    }

    /// <summary>
    ///     Caches or updates a single participant in the master storage.
    /// </summary>
    public Task SetAsync(RaceParticipantComplex item)
    {
        return _master.SetAsync(item.Id, item);
    }

    /// <summary>
    ///     Caches a list of participants grouped by race, and registers all items in the master store.
    /// </summary>
    public async Task SetRaceAsync(string raceId, List<RaceParticipantComplex> items)
    {
        foreach (var item in items)
        {
            await _master.SetAsync(item.Id, item);
        }
        await _raceIndex.SetAsync(raceId, items);
    }

    /// <summary>
    ///     Caches a list of participants grouped by driver, and registers all items in the master store.
    /// </summary>
    public async Task SetDriverAsync(string driverId, List<RaceParticipantComplex> items)
    {
        foreach (var item in items)
        {
            await _master.SetAsync(item.Id, item);
        }
        await _driverIndex.SetAsync(driverId, items);
    }

    /// <summary>
    ///     Caches a list of participants grouped by horse, and registers all items in the master store.
    /// </summary>
    public async Task SetHorseAsync(string horseId, List<RaceParticipantComplex> items)
    {
        foreach (var item in items)
        {
            await _master.SetAsync(item.Id, item);
        }
        await _horseIndex.SetAsync(horseId, items);
    }

    /// <summary>
    ///     Caches a list of participants grouped by trainer, and registers all items in the master store.
    /// </summary>
    public async Task SetTrainerAsync(string trainerId, List<RaceParticipantComplex> items)
    {
        foreach (var item in items)
        {
            await _master.SetAsync(item.Id, item);
        }
        await _trainerIndex.SetAsync(trainerId, items);
    }
}