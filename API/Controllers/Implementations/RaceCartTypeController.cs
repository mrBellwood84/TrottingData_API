using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

/// <inheritdoc />
public class RaceCartTypeController(
    IRepositoryService<RaceCartTypeEntity, RaceCartTypeComplex> repository)
    : ModelController<RaceCartTypeEntity, RaceCartTypeComplex>(repository)
{
}