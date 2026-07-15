using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class HorseTypeController(IRepositoryService<HorseTypeEntity, HorseTypeComplex> repository)
    : ReadFullModelController<HorseTypeEntity, HorseTypeComplex>(repository)
{
}