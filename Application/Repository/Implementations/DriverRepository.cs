using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class DriverRepository(
    CacheService<DriverEntity> entityCache,
    CacheService<DriverComplex> complexCache,
    SourcedCacheService<DriverEntity> sourceEntityCache,
    SourcedCacheService<DriverComplex> sourceComplexCache,
    ISourcedDbService<DriverEntity, DriverComplex> dbService,
    ModelPolicy<DriverEntity> modelPolicy)
    : SourcedRepositoryService<DriverEntity, DriverComplex>(entityCache, complexCache, sourceEntityCache,
        sourceComplexCache, dbService, modelPolicy)
{
}