using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class RaceStartTypeRepository(
    CacheService<RaceStartTypeEntity> simpleCache,
    CacheService<RaceStartTypeComplex> complexCache,
    IDbService<RaceStartTypeEntity, RaceStartTypeComplex> dbService,
    ModelPolicy<RaceStartTypeEntity> modelPolicy)
    : RepositoryService<RaceStartTypeEntity, RaceStartTypeComplex>(simpleCache, complexCache, dbService, modelPolicy)
{
}