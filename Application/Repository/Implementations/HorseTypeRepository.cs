using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Simple;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class HorseTypeRepository(
    CacheService<HorseTypeEntity> simpleCache,
    CacheService<HorseTypeComplex> complexCache,
    IDbService<HorseTypeEntity, HorseTypeComplex> dbService)
    : RepositoryService<HorseTypeEntity, HorseTypeComplex>(simpleCache, complexCache, dbService)
{
}