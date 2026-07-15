using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;

namespace API.Controllers.Base;

/// <summary>
///     A base controller extending single read operations to support lookups by external source identifiers
///     for both flat entities and complex models.
/// </summary>
/// <typeparam name="TEntity">The flat entity model type, implementing <see cref="ISourcedEntity" />.</typeparam>
/// <typeparam name="TComplex">The aggregated complex model type, implementing <see cref="ISourcedEntity" />.</typeparam>
/// <param name="repository">The sourced repository service responsible for data retrieval using external identifiers.</param>
public class ReadSourceModelController<TEntity, TComplex>(IReadSourcedRepository<TEntity, TComplex> repository)
    : ReadSingleModelController<TEntity, TComplex>(repository)
    where TEntity : ISourcedEntity
    where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a single flat entity by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external source identifier of the entity.</param>
    /// <returns>The flat entity if found; otherwise, a 404 Not Found response.</returns>
    /// <response code="200">Returns the requested flat entity.</response>
    /// <response code="404">If no entity with the specified source ID exists.</response>
    [HttpGet("entity/source/{sourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TEntity>> GetEntityBySourceIdAsync([FromRoute] string sourceId)
    {
        var data = await repository.GetEntityBySourceIdAsync(sourceId);
        if (data is null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves a single complex model by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external source identifier of the complex model.</param>
    /// <returns>The complex model if found; otherwise, a 404 Not Found response.</returns>
    /// <response code="200">Returns the requested complex model.</response>
    /// <response code="404">If no complex model with the specified source ID exists.</response>
    [HttpGet("complex/source/{sourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TComplex>> GetComplexBySourceIdAsync([FromRoute] string sourceId)
    {
        var data = await repository.GetComplexBySourceIdAsync(sourceId);
        if (data is null) return NotFound();
        return Ok(data);
    }
}