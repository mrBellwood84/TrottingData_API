using Persistence.Exceptions;
using Persistence.Services;

namespace Persistence.Interfaces;

public interface IReadSingleDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Retrieves a single simple entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The retrieved <typeparamref name="TEntity" /> if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadSingleDbService{TEntity,TComplex}.SqlSelectEntityById" /> is not defined by the subclass.
    /// </exception>
    public Task<TEntity?> GetSingleEntityByIdAsync(string id);

    /// <summary>
    ///     Retrieves a single complex model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the complex model to retrieve.</param>
    /// <returns>The retrieved <typeparamref name="TComplex" /> if found; otherwise, <see langword="null" />.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadSingleDbService{TEntity,TComplex}.SqlSelectComplexById" /> is not defined by the subclass.
    /// </exception>
    public Task<TComplex?> GetSingleComplexByIdAsync(string id);
}