using Models.Shared;

namespace Application.Repository.Interfaces;

public interface IRepositoryService<TSimple, TComplex>
{
    /// <summary>
    /// Retrieves a list of all available IDs for this entity type.
    /// </summary>
    /// <remarks>
    /// This query bypasses the cache and queries the database directly, 
    /// as ID listings are lightweight and frequently used for synchronization.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="IdModel"/>s.</returns>
    Task<List<IdModel>> GetIdsAsync();

    /// <summary>
    /// Retrieves all simple model representations.
    /// </summary>
    /// <remarks>
    /// This method checks the cache first (Cache-Aside). If the cache is empty, 
    /// it fetches the data from the database, populates the cache for future requests, and returns the result.
    /// </remarks>
    /// <returns>A list of simple models of type <typeparamref name="TSimple"/>.</returns>
    Task<List<TSimple>> GetAllSimplesAsync();

    /// <summary>
    /// Retrieves a single simple model representation by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Looks up the item in the cache first. If it is a cache miss, the item is loaded from 
    /// the database, written to the cache, and returned.
    /// </remarks>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The found simple model of type <typeparamref name="TSimple"/>, or <see langword="null"/> if not found.</returns>
    Task<TSimple?> GetSimpleByIdAsync(string id);

    /// <summary>
    /// Retrieves all complex model representations.
    /// </summary>
    /// <remarks>
    /// This method checks the complex cache first. If the cache is empty, 
    /// it fetches the complex representations from the database, populates the cache, and returns the list.
    /// </remarks>
    /// <returns>A list of complex models of type <typeparamref name="TComplex"/>.</returns>
    Task<List<TComplex>> GetAllComplexAsync();

    /// <summary>
    /// Retrieves a single complex model representation by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Looks up the item in the complex cache first. If it is a cache miss, the complex representation 
    /// is loaded from the database, written to the complex cache, and returned.
    /// </remarks>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The found complex model of type <typeparamref name="TComplex"/>, or <see langword="null"/> if not found.</returns>
    Task<TComplex?> GetComplexByIdAsync(string id);
}