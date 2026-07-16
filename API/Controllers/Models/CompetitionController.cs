using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class CompetitionController(IReadAllRepository<CompetitionEntity, CompetitionComplex> repository)
    : ReadAllModelController<CompetitionEntity, CompetitionComplex>(repository)
{
}