using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Simple;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class RaceGamblingTypeRepository(
    CacheService<RaceGamblingTypeEntity> simpleCache,
    CacheService<RaceGamblingTypeComplex> complexCache,
    IDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> dbService)
    : RepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(simpleCache, complexCache, dbService)
{
}