using Models.Complex;
using Models.Simple;
using Persistence.Interfaces;

namespace API.Controllers.Implementations;

public class DriverLicenseController(IDbService<DriverLicenseEntity, DriverLicenseComplex> dbService) 
    : ModelController<DriverLicenseEntity, DriverLicenseComplex>(dbService)
{
    
}