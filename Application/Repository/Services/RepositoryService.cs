using Application.Cache.Services;
using Application.Repository.Interfaces;
using Models.Entities;
using Models.Interfaces;
using Persistence.Interfaces;

namespace Application.Repository.Services;

public class RepositoryService<TSimple, TComplex>(
    CacheService<TSimple> simpleCache, 
    CacheService<TComplex> complexCache,
    IDbService<TSimple,TComplex> dbService) : IRepositoryService<TSimple, TComplex> where TSimple : IDbItem where TComplex : IDbItem
{
    public async Task<List<IdModel>> GetIdsAsync()
    {
        return await dbService.GetIdsAsync();
    }

    public async Task<List<TSimple>> GetAllSimplesAsync()
    {
        var cacheData = await simpleCache.GetAll();
        if (cacheData.Count > 0) return cacheData;

        var dbData = await dbService.GetAllSimpleAsync();
        foreach (var item in dbData)
            await simpleCache.Set(item.Id, item);
        return dbData;
    }
    
    public async Task<TSimple?> GetSimpleByIdAsync(string id)
    {
        var cacheData = await simpleCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetSimpleByIdAsync(id);
        if (dbData is null) return default;
        await simpleCache.Set(dbData.Id, dbData);
        return dbData;
    }

    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        var cacheData = await complexCache.GetAll();
        if (cacheData.Count > 0) return cacheData;

        var dbData = await dbService.GetAllComplexAsync();
        foreach (var item in dbData)
            await complexCache.Set(item.Id, item);
        return dbData;
    }

    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.Get(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexByIdAsync(id);
        if  (dbData is null) return default;
        await complexCache.Set(dbData!.Id, dbData);
        return dbData;
    }
}