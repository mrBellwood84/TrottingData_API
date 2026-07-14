using Models.Interfaces;
using Models.Shared;
using Persistence.Exceptions;
using Persistence.Services;

namespace Persistence.Interfaces;

public interface ISourcedDbService<TEntity, TComplex> : IDbService<TEntity, TComplex> 
    where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a single flat entity by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>The matching <typeparamref name="TEntity" />, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="SourcedDbService{TEntity,TComplex}.QueryEntityBySourceId" /> has not been configured.
    /// </exception>
    Task<TEntity?> GetEntityBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves a specific complex model by its external source identifier. Relies on internal mapping logic.
    /// </summary>
    /// <param name="sourceId">The external unique identifier from the source system.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model, or <see langword="null" /> if not found.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="SourcedDbService{TEntity,TComplex}.QueryComplexBySourceId" /> has not been configured.
    /// </exception>
    Task<TComplex?> GetComplexBySourceIdAsync(string sourceId);
}