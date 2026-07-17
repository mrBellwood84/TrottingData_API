using Models.Interfaces;

namespace Application.Cache.Interfaces;

/// <summary>
///     Defines the caching contract for race participant data of type <typeparamref name="T"/>.
///     Supports isolated, grouped indexing paths for races, drivers, horses, and trainers.
/// </summary>
/// <typeparam name="T">The model or entity type representing a race participant, implementing <see cref="IEntity"/>.</typeparam>
public interface IRaceParticipantCache<T> where T : IEntity
{
    /// <summary>
    ///     Retrieves a single cached participant by its unique identifier.
    /// </summary>
    /// <param name="key">The unique identifier (Id) of the participant.</param>
    /// <returns>The cached participant of type <typeparamref name="T"/>, or <c>null</c> if not found.</returns>
    Task<T?> GetAsync(string key);

    /// <summary>
    ///     Retrieves all cached participants associated with a specific race.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <returns>A list of cached participants of type <typeparamref name="T"/>, or <c>null</c> if the race index is cold.</returns>
    Task<List<T>?> GetByRaceAsync(string raceId);

    /// <summary>
    ///     Retrieves all cached race participations associated with a specific driver.
    /// </summary>
    /// <param name="driverId">The unique identifier of the driver.</param>
    /// <returns>A list of cached participations of type <typeparamref name="T"/>, or <c>null</c> if the driver index is cold.</returns>
    Task<List<T>?> GetByDriverAsync(string driverId);

    /// <summary>
    ///     Retrieves all cached race participations associated with a specific horse.
    /// </summary>
    /// <param name="horseId">The unique identifier of the horse.</param>
    /// <returns>A list of cached participations of type <typeparamref name="T"/>, or <c>null</c> if the horse index is cold.</returns>
    Task<List<T>?> GetByHorseAsync(string horseId);

    /// <summary>
    ///     Retrieves all cached race participations associated with a specific trainer.
    /// </summary>
    /// <param name="trainerId">The unique identifier of the trainer.</param>
    /// <returns>A list of cached participations of type <typeparamref name="T"/>, or <c>null</c> if the trainer index is cold.</returns>
    Task<List<T>?> GetByTrainAsync(string trainerId);

    /// <summary>
    ///     Stores or updates a single participant in the master storage.
    /// </summary>
    /// <param name="item">The participant item to cache.</param>
    Task SetAsync(T item);

    /// <summary>
    ///     Caches a list of participants grouped by race, and indexes them individually in the master store.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <param name="items">The list of participants to cache.</param>
    Task SetRaceAsync(string raceId, List<T> items);

    /// <summary>
    ///     Caches a list of participants grouped by driver, and indexes them individually in the master store.
    /// </summary>
    /// <param name="driverId">The unique identifier of the driver.</param>
    /// <param name="items">The list of participations to cache.</param>
    Task SetDriverAsync(string driverId, List<T> items);

    /// <summary>
    ///     Caches a list of participants grouped by horse, and indexes them individually in the master store.
    /// </summary>
    /// <param name="horseId">The unique identifier of the horse.</param>
    /// <param name="items">The list of participations to cache.</param>
    Task SetHorseAsync(string horseId, List<T> items);

    /// <summary>
    ///     Caches a list of participants grouped by trainer, and indexes them individually in the master store.
    /// </summary>
    /// <param name="trainerId">The unique identifier of the trainer.</param>
    /// <param name="items">The list of participations to cache.</param>
    Task SetTrainerAsync(string trainerId, List<T> items);
}