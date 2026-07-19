using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface IRaceResultDbService : IReadSingleDbService<RaceResultEntity, RaceResultComplex>
{
    /// <summary>
    ///     Retrieves a single flat race result entity associated with a specific race participant ID.
    /// </summary>
    Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId);

    /// <summary>
    ///     Retrieves a single complex race result model associated with a specific race participant ID.
    /// </summary>
    Task<RaceResultComplex?> GetComplexByParticipantIdAsync(string participantId);
}