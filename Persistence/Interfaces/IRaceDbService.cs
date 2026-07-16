using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface IRaceDbService : IReadAllDbService<RaceEntity, RaceComplex>
{
    /// <summary>
    ///     Retrieves a single flat race entity associated with the given competition ID.
    /// </summary>
    Task<List<RaceEntity>> GetEntityByCompetitionIdAsync(string competitionId);

    /// <summary>
    ///     Retrieves all races associated with a specific competition, fully hydated with
    ///     participants, drivers, horses, results, and gambling types.
    /// </summary>
    Task<List<RaceComplex>> GetComplexByCompetitionIdAsync(string competitionId);
}