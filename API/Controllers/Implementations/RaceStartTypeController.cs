using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

public class RaceStartTypeController(
    IRepositoryService<RaceStartTypeEntity, RaceStartTypeComplex> repository)
    : ModelController<RaceStartTypeEntity, RaceStartTypeComplex>(repository)
{
}