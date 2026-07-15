using Models.Shared;
using Persistence.Exceptions;

namespace Persistence.Services;

public interface IReadAllDbService<TEntity, TComplex> : IReadSingleDbService<TEntity, TComplex>
{
    /// <summary>
    ///     Retrieves all available unique identifiers for the entity type.
    /// </summary>
    /// <returns>A list of <see cref="IdModel" /> instances containing the active IDs.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadAllDbService{TEntity,TComplex}.SqlSelectIds" /> is not defined by the subclass.
    /// </exception>
    Task<List<IdModel>> GetIdsAsync();

    /// <summary>
    ///     Retrieves all instances of the simple entity from the database.
    /// </summary>
    /// <returns>A list containing all retrieved <typeparamref name="TEntity" /> instances.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadAllDbService{TEntity,TComplex}.SqlSelectEntities" /> is not defined by the subclass.
    /// </exception>
    Task<List<TEntity>> GetEntitiesAsync();

    /// <summary>
    ///     Retrieves all instances of the complex model from the database.
    ///     Uses the virtual list helper to allow for custom multi-mapping overrides.
    /// </summary>
    /// <returns>A list containing all populated <typeparamref name="TComplex" /> models.</returns>
    /// <exception cref="PersistenceMissingQueryException">
    ///     Thrown when
    ///     <see cref="ReadAllDbService{TEntity,TComplex}.SqlSelectComplex" /> is not defined by the subclass.
    /// </exception>
    Task<List<TComplex>> GetComplexEntitiesAsync();
}