using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class RaceStartTypeController(IRepositoryService<RaceStartTypeEntity, RaceStartTypeComplex> repository)
    : ReadFullModelController<RaceStartTypeEntity, RaceStartTypeComplex>(repository)
{
}