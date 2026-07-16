using Models.Complex;
using Models.Entity;

namespace Application.Repository.Interfaces;

public interface IRaceRepository : IReadSingleRepository<RaceEntity, RaceComplex>
{
    /// <summary>
    ///     Retrieves all flat race entities associated with a specific competition.
    /// </summary>
    Task<List<RaceEntity>?> GetRaceEntitiesByCompetitionIdAsync(string competitionId);

    /// <summary>
    ///     Retrieves all complex race models associated with a specific competition.
    /// </summary>
    Task<List<RaceComplex>?> GetRaceComplexByCompetitionIdAsync(string competitionId);
}