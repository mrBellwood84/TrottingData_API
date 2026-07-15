using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class HorseController(ISourcedRepositoryService<HorseEntity, HorseComplex> repository)
    : ReadSourceModelController<HorseEntity, HorseComplex>(repository)
{
}