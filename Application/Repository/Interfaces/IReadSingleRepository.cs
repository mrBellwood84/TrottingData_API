using Models.Interfaces;

namespace Application.Repository.Interfaces;

public interface IReadSingleRepository<TEntity, TComplex> where TEntity : IEntity where TComplex : IEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its identifier, checking the cache first before querying the database.
    /// </summary>
    Task<TEntity?> GetEntityByIdAsync(string id);

    /// <summary>
    ///     Retrieves a complex model by its identifier, checking the cache first before querying the database.
    /// </summary>
    Task<TComplex?> GetComplexByIdAsync(string id);
}