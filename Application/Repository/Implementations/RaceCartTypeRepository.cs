using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class RaceCartTypeRepository(
    CacheService<RaceCartTypeEntity> simpleCache,
    CacheService<RaceCartTypeComplex> complexCache,
    IDbService<RaceCartTypeEntity, RaceCartTypeComplex> dbService,
    ModelPolicy<RaceCartTypeEntity> modelPolicy)
    : RepositoryService<RaceCartTypeEntity, RaceCartTypeComplex>(simpleCache, complexCache, dbService, modelPolicy)
{
}