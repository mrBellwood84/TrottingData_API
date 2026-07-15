using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface IRaceDbService : IReadSingleDbService<RaceEntity, RaceComplex>
{
    /// <summary>
    ///     Retrieves a list of flat race entities belonging to a competition.
    /// </summary>
    Task<IEnumerable<RaceEntity>> GetEntitiesByCompetitionIdAsync(string competitionId);

    /// <summary>
    ///     Retrieves deep, complex object trees for all races in a specific competition.
    /// </summary>
    Task<IEnumerable<RaceComplex>> GetComplexesByCompetitionIdAsync(string competitionId);
}