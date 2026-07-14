using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class HorseSexRepository(
    CacheService<HorseSexEntity> simpleCache,
    CacheService<HorseSexComplex> complexCache,
    IDbService<HorseSexEntity, HorseSexComplex> dbService)
    : RepositoryService<HorseSexEntity, HorseSexComplex>(simpleCache, complexCache, dbService)
{
}