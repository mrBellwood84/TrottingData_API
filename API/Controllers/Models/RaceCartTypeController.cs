using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class RaceCartTypeController(IRepositoryService<RaceCartTypeEntity, RaceCartTypeComplex> repository)
    : ReadFullModelController<RaceCartTypeEntity, RaceCartTypeComplex>(repository)
{
}