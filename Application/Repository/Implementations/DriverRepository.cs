using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class DriverRepository(
    CacheService<DriverEntity> entityCache, 
    CacheService<DriverComplex> complexCache, 
    SourcedCacheService<DriverEntity> sourceEntityCache, 
    SourcedCacheService<DriverComplex> sourceComplexCache, 
    ISourcedDbService<DriverEntity, DriverComplex> dbService) 
    : SourcedRepositoryService<DriverEntity, DriverComplex>(entityCache, complexCache, sourceEntityCache, sourceComplexCache, dbService)
{
    
}