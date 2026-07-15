using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface IRaceParticipantDbService : IReadSingleDbService<RaceParticipantEntity, RaceParticipantComplex>
{
    /// <summary>
    ///     Retrieves flat participant entities belonging to a specific race.
    /// </summary>
    Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByRaceIdAsync(string raceId);

    /// <summary>
    ///     Retrieves complex participant models belonging to a specific race.
    /// </summary>
    Task<IEnumerable<RaceParticipantComplex>> GetComplexesByRaceIdAsync(string raceId);

    /// <summary>
    ///     Retrieves flat participant entities associated with a driver's source ID.
    /// </summary>
    Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByDriverSourceIdAsync(string driverSourceId);

    /// <summary>
    ///     Retrieves complex participant models associated with a driver's source ID.
    /// </summary>
    Task<IEnumerable<RaceParticipantComplex>> GetComplexesByDriverSourceIdAsync(string driverSourceId);

    /// <summary>
    ///     Retrieves flat participant entities associated with a horse's source ID.
    /// </summary>
    Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByHorseSourceIdAsync(string horseSourceId);

    /// <summary>
    ///     Retrieves complex participant models associated with a horse's source ID.
    /// </summary>
    Task<IEnumerable<RaceParticipantComplex>> GetComplexesByHorseSourceIdAsync(string horseSourceId);

    /// <summary>
    ///     Retrieves flat participant entities associated with a trainer's source ID.
    /// </summary>
    Task<IEnumerable<RaceParticipantEntity>> GetEntitiesByTrainerSourceIdAsync(string trainerSourceId);

    /// <summary>
    ///     Retrieves complex participant models associated with a trainer's source ID.
    /// </summary>
    Task<IEnumerable<RaceParticipantComplex>> GetComplexesByTrainerSourceIdAsync(string trainerSourceId);
}