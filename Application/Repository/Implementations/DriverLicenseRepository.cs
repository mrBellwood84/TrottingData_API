using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

/// <inheritdoc />
public class DriverLicenseRepository(
    CacheService<DriverLicenseEntity> simpleCache,
    CacheService<DriverLicenseComplex> complexCache,
    IDbService<DriverLicenseEntity, DriverLicenseComplex> dbService,
    ModelPolicy<DriverLicenseEntity> modelPolicy)
    : RepositoryService<DriverLicenseEntity, DriverLicenseComplex>(simpleCache, complexCache, dbService, modelPolicy)
{
}