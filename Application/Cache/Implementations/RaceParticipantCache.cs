using Application.Cache.Interfaces;
using Application.Cache.Services;
using Models.Interfaces;

namespace Application.Cache.Implementations;

/// <summary>
///     In-memory cache engine for managing race participant data of type <typeparamref name="T" />.
///     Provides isolated, grouped indexing paths for races, drivers, horses, and trainers
///     to prevent data contamination across different query vectors.
/// </summary>
/// <typeparam name="T">The model or entity type representing a race participant, implementing <see cref="IEntity" />.</typeparam>
public class RaceParticipantCache<T> : IRaceParticipantCache<T> where T : IEntity
{
    private readonly GroupedCacheService<T> _driverIndex = new();
    private readonly GroupedCacheService<T> _horseIndex = new();
    private readonly CacheService<T> _master = new();
    private readonly GroupedCacheService<T> _raceIndex = new();
    private readonly GroupedCacheService<T> _trainerIndex = new();

    /// <summary>
    ///     Retrieves a single cached participant by its unique identifier.
    /// </summary>
    /// <param name="key">The unique identifier (Id) of the participant.</param>
    /// <returns>The cached participant of type <typeparamref name="T" />, or <c>null</c> if not found.</returns>
    public Task<T?> GetAsync(string key)
    {
        return _master.GetAsync(key);
    }

    /// <summary>
    ///     Retrieves all cached participants associated with a specific race.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <returns>A list of cached participants of type <typeparamref name="T" />, or <c>null</c> if the race index is cold.</returns>
    public Task<List<T>?> GetByRaceAsync(string raceId)
    {
        return _raceIndex.GetAsync(raceId);
    }

    /// <summary>
    ///     Retrieves all cached race participations associated with a specific driver.
    /// </summary>
    /// <param name="driverId">The unique identifier of the driver.</param>
    /// <returns>A list of cached participations of type <typeparamref name="T" />, or <c>null</c> if the driver index is cold.</returns>
    public Task<List<T>?> GetByDriverAsync(string driverId)
    {
        return _driverIndex.GetAsync(driverId);
    }

    /// <summary>
    ///     Retrieves all cached race participations associated with a specific horse.
    /// </summary>
    /// <param name="horseId">The unique identifier of the horse.</param>
    /// <returns>A list of cached participations of type <typeparamref name="T" />, or <c>null</c> if the horse index is cold.</returns>
    public Task<List<T>?> GetByHorseAsync(string horseId)
    {
        return _horseIndex.GetAsync(horseId);
    }

    /// <summary>
    ///     Retrieves all cached race participations associated with a specific trainer.
    /// </summary>
    /// <param name="trainerId">The unique identifier of the trainer.</param>
    /// <returns>
    ///     A list of cached participations of type <typeparamref name="T" />, or <c>null</c> if the trainer index is
    ///     cold.
    /// </returns>
    public Task<List<T>?> GetByTrainAsync(string trainerId)
    {
        return _trainerIndex.GetAsync(trainerId);
    }

    /// <summary>
    ///     Stores or updates a single participant in the master storage.
    /// </summary>
    /// <param name="item">The participant item to cache.</param>
    public Task SetAsync(T item)
    {
        return _master.SetAsync(item.Id, item);
    }

    /// <summary>
    ///     Caches a list of participants grouped by race, and indexes them individually in the master store.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <param name="items">The list of participants to cache.</param>
    public async Task SetRaceAsync(string raceId, List<T> items)
    {
        foreach (var item in items) await _master.SetAsync(item.Id, item);
        await _raceIndex.SetAsync(raceId, items);
    }

    /// <summary>
    ///     Caches a list of participants grouped by driver, and indexes them individually in the master store.
    /// </summary>
    /// <param name="driverId">The unique identifier of the driver.</param>
    /// <param name="items">The list of participations to cache.</param>
    public async Task SetDriverAsync(string driverId, List<T> items)
    {
        foreach (var item in items) await _master.SetAsync(item.Id, item);
        await _driverIndex.SetAsync(driverId, items);
    }

    /// <summary>
    ///     Caches a list of participants grouped by horse, and indexes them individually in the master store.
    /// </summary>
    /// <param name="horseId">The unique identifier of the horse.</param>
    /// <param name="items">The list of participations to cache.</param>
    public async Task SetHorseAsync(string horseId, List<T> items)
    {
        foreach (var item in items) await _master.SetAsync(item.Id, item);
        await _horseIndex.SetAsync(horseId, items);
    }

    /// <summary>
    ///     Caches a list of participants grouped by trainer, and indexes them individually in the master store.
    /// </summary>
    /// <param name="trainerId">The unique identifier of the trainer.</param>
    /// <param name="items">The list of participations to cache.</param>
    public async Task SetTrainerAsync(string trainerId, List<T> items)
    {
        foreach (var item in items) await _master.SetAsync(item.Id, item);
        await _trainerIndex.SetAsync(trainerId, items);
    }
}