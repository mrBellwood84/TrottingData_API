using Models.Interfaces;

namespace Application.Repository.Interfaces;

public interface IReadSourcedRepository<TEntity, TComplex>
    : IReadSingleRepository<TEntity, TComplex>
    where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its external source identifier, checking the sourced cache first.
    /// </summary>
    Task<TEntity?> GetEntityBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves a complex model by its external source identifier, checking the sourced cache first.
    /// </summary>
    Task<TComplex?> GetComplexBySourceIdAsync(string sourceId);
}