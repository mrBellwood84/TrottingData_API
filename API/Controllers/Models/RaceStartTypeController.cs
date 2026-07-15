using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

/// <summary>
///     Provides read-only endpoints for managing and retrieving race start type data,
///     supporting identity lookups, flat entity lists, and complex domain models.
/// </summary>
public class RaceStartTypeController(IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex> repository)
    : ReadFullModelController<RaceStartTypeEntity, RaceStartTypeComplex>(repository)
{
}