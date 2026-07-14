using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Implementations;

public class HorseController(ISourcedRepositoryService<HorseEntity, HorseComplex> repository) 
    : SourceModelController<HorseEntity, HorseComplex>(repository)
{
    
}