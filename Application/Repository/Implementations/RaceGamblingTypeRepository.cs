using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class RaceGamblingTypeRepository(
    CacheService<RaceGamblingTypeEntity> simpleCache,
    CacheService<RaceGamblingTypeComplex> complexCache,
    IDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> dbService,
    ModelPolicy<RaceGamblingTypeEntity> modelPolicy)
    : RepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(simpleCache, complexCache, dbService,
        modelPolicy)
{
}