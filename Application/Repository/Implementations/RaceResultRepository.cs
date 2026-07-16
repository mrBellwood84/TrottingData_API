using Application.Cache.Interfaces;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <summary>
///     Orchestrates race result data operations by coordinating lookups between
///     the database services and dual-indexed caches for both entities and complex models.
///     Enforces strict data integrity policies via fail-fast mechanisms.
/// </summary>
public class RaceResultRepository(
    IRaceResultDbService dbService,
    IRaceResultCache<RaceResultEntity> entityCache,
    IRaceResultCache<RaceResultsComplex> complexCache) : IRaceResultRepository
{
    /// <summary>
    ///     Retrieves a flat race result entity by its unique identifier, checking the cache first.
    /// </summary>
    public async Task<RaceResultEntity?> GetEntityByIdAsync(string id)
    {
        var cacheData = await entityCache.GetAsync(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityByIdAsync(id);
        if (dbData is not null) await entityCache.SetAsync(dbData.RaceParticipantId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a flat race result entity by its associated participant identifier, checking the cache first.
    /// </summary>
    public async Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId)
    {
        var cacheData = await entityCache.GetByParticipantAsync(participantId);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityByParticipantIdAsync(participantId);
        if (dbData is not null) await entityCache.SetAsync(participantId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex race result by its unique identifier, checking the cache first.
    ///     Uses a deliberate fail-fast policy to assert relational database integrity.
    /// </summary>
    /// <exception cref="NullReferenceException">
    ///     Thrown if the complex model exists but the underlying flat entity is missing.
    /// </exception>
    public async Task<RaceResultsComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.GetAsync(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexByIdAsync(id);
        if (dbData is not null)
        {
            // DELIBERATE FAIL-FAST DESIGN !!!
            var entity = await GetEntityByIdAsync(id);
            await complexCache.SetAsync(entity!.RaceParticipantId, dbData);
        }

        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex race result by its associated participant identifier, checking the cache first.
    /// </summary>
    public async Task<RaceResultsComplex?> GetComplexByParticipantIdAsync(string participantId)
    {
        var cacheData = await complexCache.GetByParticipantAsync(participantId);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetComplexByParticipantIdAsync(participantId);
        if (dbData is not null) await complexCache.SetAsync(participantId, dbData);
        return dbData;
    }
}