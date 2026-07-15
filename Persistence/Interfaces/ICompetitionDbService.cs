using Models.Complex;
using Models.Entity;

namespace Persistence.Interfaces;

public interface ICompetitionDbService : IReadAllDbService<CompetitionEntity, CompetitionComplex>
{
    /// <summary>
    ///     Retrieves a list of all competition IDs in the database.
    /// </summary>
    Task<IEnumerable<string>> GetAllIdsAsync();

    /// <summary>
    ///     Retrieves flat entities based on a list of specific IDs.
    /// </summary>
    Task<IEnumerable<CompetitionEntity>> GetEntitiesByIdsAsync(IEnumerable<string> ids);

    /// <summary>
    ///     Retrieves complete, deep object trees based on a list of specific IDs.
    /// </summary>
    Task<IEnumerable<CompetitionComplex>> GetComplexesByIdsAsync(IEnumerable<string> ids);
}