using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class RaceGamblingTypeController(IRepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex> repository)
    : ReadFullModelController<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(repository)
{
}