using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface IRaceParticipantDbService : IReadSingleDbService<RaceParticipantEntity, RaceParticipantComplex>
{
    /// <summary>
    ///     Retrieves a list of flat participant entities registered for a specific race.
    /// </summary>
    Task<List<RaceParticipantEntity>> GetEntitiesByRaceIdAsync(string raceId);

    /// <summary>
    ///     Retrieves a list of flat participant entities associated with a specific driver source ID.
    /// </summary>
    Task<List<RaceParticipantEntity>> GetEntitiesByDriverSourceIdAsync(string driverSourceId);

    /// <summary>
    ///     Retrieves a list of flat participant entities associated with a specific horse source ID.
    /// </summary>
    Task<List<RaceParticipantEntity>> GetEntitiesByHorseSourceIdAsync(string horseSourceId);

    /// <summary>
    ///     Retrieves a list of flat participant entities associated with a specific trainer source ID.
    /// </summary>
    Task<List<RaceParticipantEntity>> GetEntitiesByTrainerSourceIdAsync(string trainerSourceId);

    /// <summary>
    ///     Retrieves all participants for a specific race, fully hydrated with drivers, horses, trainers, and results.
    /// </summary>
    Task<List<RaceParticipantComplex>> GetComplexesByRaceIdAsync(string raceId);

    /// <summary>
    ///     Retrieves all race appearances for a driver, fully hydrated with related domain complexes.
    /// </summary>
    Task<List<RaceParticipantComplex>> GetComplexesByDriverSourceIdAsync(string driverSourceId);

    /// <summary>
    ///     Retrieves all race appearances for a specific horse, fully hydrated with related domain complexes.
    /// </summary>
    Task<List<RaceParticipantComplex>> GetComplexesByHorseSourceIdAsync(string horseSourceId);

    /// <summary>
    ///     Retrieves all race entries managed by a specific trainer, fully hydrated with related domain complexes.
    /// </summary>
    Task<List<RaceParticipantComplex>> GetComplexesByTrainerSourceIdAsync(string trainerSourceId);
}