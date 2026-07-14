using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

public class RaceGamblingTypeController(
    IRepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> repository)
    : ModelController<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(repository)
{
}