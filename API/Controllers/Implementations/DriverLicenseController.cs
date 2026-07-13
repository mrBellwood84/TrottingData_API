using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Interfaces;

namespace API.Controllers.Implementations;

public class DriverLicenseController(IDbService<DriverLicenseEntity, DriverLicenseComplex> dbService, EntityPolicy<DriverLicenseEntity> policy) 
    : ModelController<DriverLicenseEntity, DriverLicenseComplex>(dbService, policy)
{
    
}