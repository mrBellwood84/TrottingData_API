using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;

namespace API.Controllers;

/// <summary>
///     An abstract base controller for managing sourced entities, exposing endpoints 
///     to retrieve data by their external source identifiers.
/// </summary>
/// <typeparam name="TEntity">The flat entity type. Must implement <see cref="ISourcedEntity"/>.</typeparam>
/// <typeparam name="TComplex">The complex model type. Must implement <see cref="ISourcedEntity"/>.</typeparam>
/// <param name="repository">The sourced repository service handling the data flow.</param>
public abstract class SourceModelController<TEntity, TComplex>(
    ISourcedRepositoryService<TEntity, TComplex> repository) 
    : ModelController<TEntity, TComplex>(repository) 
    where TEntity : ISourcedEntity 
    where TComplex : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a flat entity by its external Source ID.
    /// </summary>
    /// <param name="sourceId">The external system identifier.</param>
    /// <returns>The matching entity if found; otherwise, a 404 Not Found response.</returns>
    [HttpGet("entity/source/{sourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TEntity>> GetEntityBySourceIdAsync(string sourceId)
    {
        var data = await repository.GetEntityBySourceIdAsync(sourceId);
        if (data is null) return NotFound();
        
        return Ok(data);
    }

    /// <summary>
    ///     Retrieves a complex model by its external Source ID.
    /// </summary>
    /// <param name="sourceId">The external system identifier.</param>
    /// <returns>The matching complex model if found; otherwise, a 404 Not Found response.</returns>
    [HttpGet("complex/source/{sourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TComplex>> GetComplexBySourceIdAsync(string sourceId)
    {
        var data = await repository.GetComplexBySourceIdAsync(sourceId);
        if (data is null) return NotFound();
        
        return Ok(data);
    }
}