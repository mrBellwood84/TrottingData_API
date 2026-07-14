using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class HorseRepository(
    CacheService<HorseEntity> entityCache, 
    CacheService<HorseComplex> complexCache, 
    SourcedCacheService<HorseEntity> sourceEntityCache, 
    SourcedCacheService<HorseComplex> sourceComplexCache, 
    ISourcedDbService<HorseEntity, HorseComplex> dbService) 
    : SourcedRepositoryService<HorseEntity, HorseComplex>(entityCache, complexCache, sourceEntityCache, sourceComplexCache, dbService)
{
    
}