using API.Controllers.Base;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

/// <summary>
///     Provides read-only endpoints for managing and retrieving horse data,
///     supporting lookups by both internal identifiers and external source identifiers.
/// </summary>
public class HorseController(IReadSourceRepository<HorseEntity, HorseComplex> repository)
    : ReadSourceModelController<HorseEntity, HorseComplex>(repository)
{
}