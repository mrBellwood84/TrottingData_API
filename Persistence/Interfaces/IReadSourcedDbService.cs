using Persistence.Exceptions;

namespace Persistence.Services;

public interface IReadSourcedDbService<TEntity, TComplex> : IReadSingleDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Retrieves a single simple entity by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external source identifier of the entity.</param>
    /// <returns>The retrieved <typeparamref name="TEntity" /> if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadSourcedDbService{TEntity,TComplex}.SqlSelectEntityBySourceId" /> is not defined by the subclass.
    /// </exception>
    Task<TEntity?> GetEntityBySourceIdAsync(string sourceId);

    /// <summary>
    ///     Retrieves a single complex model by its external source identifier.
    ///     Uses the virtual helper to allow specialized multi-mapping configurations in subclasses.
    /// </summary>
    /// <param name="sourceId">The external source identifier of the complex model.</param>
    /// <returns>The populated <typeparamref name="TComplex" /> model if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadSourcedDbService{TEntity,TComplex}.SqlSelectComplexBySourceId" /> is not defined by the subclass.
    /// </exception>
    Task<TComplex?> GetComplexBySourceIdAsync(string sourceId);
}