using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class RaceCourseRepository(
    CacheService<RaceCourseEntity> simpleCache,
    CacheService<RaceCourseComplex> complexCache,
    IDbService<RaceCourseEntity, RaceCourseComplex> dbService,
    ModelPolicy<RaceCourseEntity> modelPolicy)
    : RepositoryService<RaceCourseEntity, RaceCourseComplex>(simpleCache, complexCache, dbService, modelPolicy)
{
}