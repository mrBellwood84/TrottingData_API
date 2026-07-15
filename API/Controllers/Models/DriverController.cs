using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class DriverController(ISourcedRepositoryService<DriverEntity, DriverComplex> repository)
    : ReadSourceModelController<DriverEntity, DriverComplex>(repository)
{
}