using Application.Cache.Services;
using Application.Repository.Services;
using Models.Complex;
using Models.Simple;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class DriverLicenseRepository(
    CacheService<DriverLicenseEntity> simpleCache, 
    CacheService<DriverLicenseComplex> complexCache, 
    IDbService<DriverLicenseEntity, DriverLicenseComplex> dbService) 
    : RepositoryService<DriverLicenseEntity, DriverLicenseComplex>(simpleCache, complexCache, dbService)
{
    
}