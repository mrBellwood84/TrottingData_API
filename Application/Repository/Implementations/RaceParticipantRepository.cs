using Application.Cache.Interfaces;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

// todo : set interface with read single repo inheritance
/// <summary>
///     Repository responsible for orchestrating read operations for race participants.
///     Handles data flow between the persistent database service and the in-memory cache layers
///     for both flat <see cref="RaceParticipantEntity"/> and rich <see cref="RaceParticipantComplex"/> models.
/// </summary>
public class RaceParticipantRepository(
    IRaceParticipantDbService dbService,
    IRaceParticipantCache<RaceParticipantEntity> entityCache,
    IRaceParticipantCache<RaceParticipantComplex> complexCache) : IRaceParticipantRepository
{
    /// <summary>
    ///     Retrieves a flat participant entity by its unique identifier, utilizing the cache if available.
    /// </summary>
    /// <param name="id">The unique identifier of the participant.</param>
    /// <returns>The <see cref="RaceParticipantEntity"/> if found; otherwise, <c>null</c>.</returns>
    public async Task<RaceParticipantEntity?> GetEntityByIdAsync(string id)
    {
        var cacheData = await entityCache.GetAsync(id);
        if (cacheData is not null) return cacheData;

        var dbData = await dbService.GetEntityByIdAsync(id);
        if (dbData is not null) await entityCache.SetAsync(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves a complex participant model by its unique identifier, utilizing the cache if available.
    /// </summary>
    /// <param name="id">The unique identifier of the participant.</param>
    /// <returns>The <see cref="RaceParticipantComplex"/> containing detailed relations if found; otherwise, <c>null</c>.</returns>
    public async Task<RaceParticipantComplex?> GetComplexByIdAsync(string id)
    {
        var cacheData = await complexCache.GetAsync(id);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetComplexByIdAsync(id);
        if (dbData is not null) await complexCache.SetAsync(dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all flat participant entities registered to a specific race.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity"/> objects, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantEntity>?> GetEntitiesByRaceIdAsync(string raceId)
    {
        var cacheData = await entityCache.GetByRaceAsync(raceId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetEntitiesByRaceIdAsync(raceId);
        if (dbData.Count == 0) return null;
        
        await entityCache.SetRaceAsync(raceId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex participant models registered to a specific race.
    /// </summary>
    /// <param name="raceId">The unique identifier of the race.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex"/> models, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantComplex>?> GetComplexByRaceIdAsync(string raceId)
    {
        var cacheData = await complexCache.GetByRaceAsync(raceId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetComplexesByRaceIdAsync(raceId);
        if (dbData.Count == 0) return null;
        
        await complexCache.SetRaceAsync(raceId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific driver.
    /// </summary>
    /// <param name="driverId">The unique source identifier of the driver.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity"/> objects, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantEntity>?> GetEntitiesByDriverAsync(string driverId)
    {
        var cacheData = await entityCache.GetByDriverAsync(driverId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetEntitiesByDriverSourceIdAsync(driverId);
        if (dbData.Count == 0) return null;
        
        await entityCache.SetDriverAsync(driverId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific driver.
    /// </summary>
    /// <param name="driverId">The unique source identifier of the driver.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex"/> models, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantComplex>?> GetComplexByDriverAsync(string driverId)
    {
        var cacheData = await complexCache.GetByDriverAsync(driverId);
        if (cacheData is not null) return cacheData;
        var dbData = await dbService.GetComplexesByDriverSourceIdAsync(driverId);
        if (dbData.Count == 0) return null;
        
        await complexCache.SetDriverAsync(driverId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific horse.
    /// </summary>
    /// <param name="horseId">The unique source identifier of the horse.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity"/> objects, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantEntity>?> GetEntitiesByHorseAsync(string horseId)
    {
        var cacheData = await entityCache.GetByHorseAsync(horseId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetEntitiesByHorseSourceIdAsync(horseId);
        if (dbData.Count == 0) return null;
        
        await entityCache.SetHorseAsync(horseId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific horse.
    /// </summary>
    /// <param name="horseId">The unique source identifier of the horse.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex"/> models, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantComplex>?> GetComplexesByHorseAsync(string horseId)
    {
        var cacheData = await complexCache.GetByHorseAsync(horseId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetComplexesByHorseSourceIdAsync(horseId);
        if (dbData.Count == 0) return null;
        
        await complexCache.SetHorseAsync(horseId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all flat race participation associated with a specific trainer.
    /// </summary>
    /// <param name="trainerId">The unique source identifier of the trainer.</param>
    /// <returns>A list of <see cref="RaceParticipantEntity"/> objects, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantEntity>?> GetEntitiesByTrainerAsync(string trainerId)
    {
        var cacheData = await entityCache.GetByTrainAsync(trainerId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetEntitiesByTrainerSourceIdAsync(trainerId);
        if (dbData.Count == 0) return null;
        
        await entityCache.SetTrainerAsync(trainerId, dbData);
        return dbData;
    }

    /// <summary>
    ///     Retrieves all complex race participation associated with a specific trainer.
    /// </summary>
    /// <param name="trainerId">The unique source identifier of the trainer.</param>
    /// <returns>A list of detailed <see cref="RaceParticipantComplex"/> models, or <c>null</c> if none exist.</returns>
    public async Task<List<RaceParticipantComplex>?> GetComplexesByTrainerAsync(string trainerId)
    {
        var cacheData = await complexCache.GetByTrainAsync(trainerId);
        if (cacheData is not null) return cacheData;
        
        var dbData = await dbService.GetComplexesByTrainerSourceIdAsync(trainerId);
        if (dbData.Count == 0) return null;
        
        await complexCache.SetTrainerAsync(trainerId, dbData);
        return dbData;
    }
}