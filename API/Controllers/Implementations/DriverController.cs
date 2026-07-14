using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

public class DriverController(ISourcedRepositoryService<DriverEntity, DriverComplex> repository) 
    : SourceModelController<DriverEntity, DriverComplex>(repository)
{
    
}