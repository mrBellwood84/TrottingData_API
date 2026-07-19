using Models.Complex;
using Models.Entity;

namespace Application.Repository.Interfaces;

public interface IRaceResultRepository : IReadSingleRepository<RaceResultEntity, RaceResultComplex>
{
    /// <summary>
    ///     Retrieves a flat race result entity by its associated participant identifier, checking the cache first.
    /// </summary>
    Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId);

    /// <summary>
    ///     Retrieves a complex race result by its associated participant identifier, checking the cache first.
    /// </summary>
    Task<RaceResultComplex?> GetComplexByParticipantIdAsync(string participantId);
}