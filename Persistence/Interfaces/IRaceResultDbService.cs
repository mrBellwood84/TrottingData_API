using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface IRaceResultDbService : IReadSingleDbService<RaceResultEntity, RaceResultsComplex>
{
    /// <summary>
    ///     Retrieves a single flat race result entity by its associated race participant identifier.
    /// </summary>
    /// <param name="participantId">The unique identifier of the race participant.</param>
    /// <returns>The flat entity representation of the race result if found; otherwise, null.</returns>
    Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId);

    /// <summary>
    ///     Retrieves a single complex race result model by its associated race participant identifier.
    /// </summary>
    /// <param name="participantId">The unique identifier of the race participant.</param>
    /// <returns>The complex representation of the race result if found; otherwise, null.</returns>
    Task<RaceResultsComplex?> GetComplexByParticipantIdAsync(string participantId);
}