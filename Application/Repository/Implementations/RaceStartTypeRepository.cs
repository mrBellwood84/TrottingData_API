using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Simple;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class RaceStartTypeRepository(
    CacheService<RaceStartTypeEntity> simpleCache,
    CacheService<RaceStartTypeComplex> complexCache,
    IDbService<RaceStartTypeEntity, RaceStartTypeComplex> dbService)
    : RepositoryService<RaceStartTypeEntity, RaceStartTypeComplex>(simpleCache, complexCache, dbService)
{
}