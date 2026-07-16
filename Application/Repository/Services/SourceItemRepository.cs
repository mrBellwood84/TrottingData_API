using Application.Cache.Interfaces;
using Application.Repository.Interfaces;
using Models.Interfaces;
using Persistence.Interfaces;

namespace Application.Repository.Services;

/// <summary>
///     Provides a repository implementation for entities and complex models that originate
///     from an external source, enabling lookups via their external source identifier
///     with dedicated cache-aside management.
/// </summary>
public class SourceItemRepository<TEntity, TComplex>(
    ISourceItemCache<TEntity> entityCache,
    ISourceItemCache<TComplex> complexCache,
    IReadSourcedDbService<TEntity, TComplex> dbService)
    : SinglesItemRepository<TEntity, TComplex>(entityCache, complexCache, dbService),
        ISourceItemRepository<TEntity, TComplex> where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its external source identifier, checking the sourced cache first.
    /// </summary>
    public async Task<TEntity?> GetEntityBySourceIdAsync(string sourceId)
    {
        var cacheData = await entityCache.GetSourceAsync(sourceId);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityBySourceIdAsync(sourceId);
        if (dbData is not null) await entityCache.SetAsync(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex model by its external source identifier, checking the sourced cache first.
    /// </summary>
    public async Task<TComplex?> GetComplexBySourceIdAsync(string sourceId)
    {
        var cacheData = await complexCache.GetAsync(sourceId);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexBySourceIdAsync(sourceId);
        if (dbData is not null) await complexCache.SetAsync(dbData);
        return dbData;
    }
}