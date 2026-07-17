using Models.Complex;
using Models.Entity;

namespace Application.Repository.Interfaces;

public interface IRaceParticipantRepository : IReadSingleRepository<RaceParticipantEntity, RaceParticipantComplex>
{
    /// <summary>
    ///     Retrieves all flat participant entities registered to a specific race.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity" /> objects, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantEntity>?> GetEntitiesByRaceIdAsync(string raceId);

    /// <summary>
    ///     Retrieves all complex participant models registered to a specific race.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex" /> models, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantComplex>?> GetComplexByRaceIdAsync(string raceId);

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific driver.
    /// </summary>
    /// <param name="driverId">The unique source identifier of the driver.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity" /> objects, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantEntity>?> GetEntitiesByDriverAsync(string driverId);

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific driver.
    /// </summary>
    /// <param name="driverId">The unique source identifier of the driver.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex" /> models, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantComplex>?> GetComplexByDriverAsync(string driverId);

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific horse.
    /// </summary>
    /// <param name="horseId">The unique source identifier of the horse.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity" /> objects, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantEntity>?> GetEntitiesByHorseAsync(string horseId);

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific horse.
    /// </summary>
    /// <param name="horseId">The unique source identifier of the horse.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex" /> models, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantComplex>?> GetComplexesByHorseAsync(string horseId);

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific trainer.
    /// </summary>
    /// <param name="trainerId">The unique source identifier of the trainer.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity" /> objects, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantEntity>?> GetEntitiesByTrainerAsync(string trainerId);

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific trainer.
    /// </summary>
    /// <param name="trainerId">The unique source identifier of the trainer.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex" /> models, or <c>null</c> if none exist.</returns>
    Task<List<RaceParticipantComplex>?> GetComplexesByTrainerAsync(string trainerId);
}