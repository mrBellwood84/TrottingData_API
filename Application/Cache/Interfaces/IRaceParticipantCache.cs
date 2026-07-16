using Models.Complex;

namespace Application.Cache.Interfaces;

public interface IRaceParticipantCache
{
    /// <summary>
    ///     Retrieves a participant by their unique identifier.
    /// </summary>
    Task<RaceParticipantComplex?> GetAsync(string key);

    /// <summary>
    ///     Retrieves all cached participants for a specific race.
    /// </summary>
    Task<List<RaceParticipantComplex>?> GetByRaceAsync(string raceId);

    /// <summary>
    ///     Retrieves all cached race participations for a specific driver.
    /// </summary>
    Task<List<RaceParticipantComplex>?> GetByDriverAsync(string driverId);

    /// <summary>
    ///     Retrieves all cached race participations for a specific horse.
    /// </summary>
    Task<List<RaceParticipantComplex>?> GetByHorseAsync(string horseId);

    /// <summary>
    ///     Retrieves all cached race participations for a specific trainer.
    /// </summary>
    Task<List<RaceParticipantComplex>?> GetByTrainAsync(string trainerId);

    /// <summary>
    ///     Caches or updates a single participant in the master storage.
    /// </summary>
    Task SetAsync(RaceParticipantComplex item);

    /// <summary>
    ///     Caches a list of participants grouped by race, and registers all items in the master store.
    /// </summary>
    Task SetRaceAsync(string raceId, List<RaceParticipantComplex> items);

    /// <summary>
    ///     Caches a list of participants grouped by driver, and registers all items in the master store.
    /// </summary>
    Task SetDriverAsync(string driverId, List<RaceParticipantComplex> items);

    /// <summary>
    ///     Caches a list of participants grouped by horse, and registers all items in the master store.
    /// </summary>
    Task SetHorseAsync(string horseId, List<RaceParticipantComplex> items);

    /// <summary>
    ///     Caches a list of participants grouped by trainer, and registers all items in the master store.
    /// </summary>
    Task SetTrainerAsync(string trainerId, List<RaceParticipantComplex> items);
}