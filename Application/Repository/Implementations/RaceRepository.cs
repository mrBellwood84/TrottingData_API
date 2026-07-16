using Application.Cache.Implementations;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Orchestrates race data operations by coordinating lookups between 
///     the database services and the in-memory cache (Cache-Aside pattern).
/// </summary>
public class RaceRepository(
    IRaceDbService dbService, 
    IRaceCache<RaceEntity> entityCache, 
    IRaceCache<RaceComplex> complexCache) : IRaceRepository
{
    /// <summary>
    ///     Retrieves a flat race entity by its ID, checking the cache first.
    /// </summary>
    public async Task<RaceEntity?> GetEntityByIdAsync(string id)
    {
        var cached = await entityCache.GetAsync(id);
        if (cached is not null) return cached;
        
        var dbData = await dbService.GetSingleEntityByIdAsync(id);
        if (dbData is not null) await entityCache.SetAsync(dbData);
        
        return dbData; 
    }

    /// <summary>
    ///     Retrieves all flat race entities associated with a specific competition.
    /// </summary>
    public async Task<List<RaceEntity>?> GetRaceEntitiesByCompetitionIdAsync(string competitionId)
    {
        var cached = await entityCache.GetByCompetitionAsync(competitionId);
        if (cached is not null && cached.Count > 0) return cached;
        
        var dbData = await dbService.GetEntityByCompetitionIdAsync(competitionId);
        if (dbData.Count <= 0) return null;
        await entityCache.SetCompetitionAsync(competitionId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex race model by its ID, checking the cache first.
    /// </summary>
    public async Task<RaceComplex?> GetComplexByIdAsync(string id)
    {
        var cached = await complexCache.GetAsync(id);
        if (cached is not null) return cached;
        
        var dbData = await dbService.GetSingleComplexByIdAsync(id);
        if (dbData is not null)  await complexCache.SetAsync(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex race models associated with a specific competition.
    /// </summary>
    public async Task<List<RaceComplex>?> GetRaceComplexByCompetitionIdAsync(string competitionId)
    {
        var cached = await complexCache.GetByCompetitionAsync(competitionId);
        if (cached is not null && cached.Count > 0) 
        {
            return cached;
        }

        var dbData = await dbService.GetComplexByCompetitionIdAsync(competitionId);
        if (dbData.Count <= 0) return null;
        await complexCache.SetCompetitionAsync(competitionId, dbData);
        return dbData;

    }
}