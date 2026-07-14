using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Models.Shared;

namespace API.Controllers;

/// <summary>
/// A generic base controller providing standard CRUD-like read operations for simple and complex model representations.
/// </summary>
/// <typeparam name="TSimple">The simple model type representing the entity. Must implement <see cref="IDbItem"/>.</typeparam>
/// <typeparam name="TComplex">The complex model type representing the entity. Must implement <see cref="IDbItem"/>.</typeparam>
/// <param name="repository">The repository service handling data flow and caching for the entities.</param>
[ApiController]
[Route("/model/[controller]")]
public class ModelController<TSimple, TComplex>(IRepositoryService<TSimple, TComplex> repository)
    : ControllerBase 
    where TSimple : IDbItem 
    where TComplex : IDbItem
{
    /// <summary>
    /// Retrieves a list of all available unique identifiers (IDs) for this entity type.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> containing a list of <see cref="IdModel"/>s.</returns>
    [HttpGet("id")]
    public async Task<ActionResult<List<IdModel>>> GetIdsAsync()
    {
        var data = await repository.GetIdsAsync();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves all simple representations of the entity.
    /// </summary>
    /// <returns>A list of simple entity models.</returns>
    [HttpGet("entity")]
    public async Task<ActionResult<List<TSimple>>> GetEntitiesAsync()
    {
        var data = await repository.GetAllSimplesAsync();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves a single simple entity representation by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The requested simple entity model.</returns>
    /// <response code="200">Returns the requested entity.</response>
    /// <response code="404">If the entity with the specified identifier was not found.</response>
    [HttpGet("entity/{id}")]
    public async Task<ActionResult<TSimple>> GetEntityAsync(string id)
    {
        var data = await repository.GetSimpleByIdAsync(id);
        if (data is null) return NotFound();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves all complex representations of the entity.
    /// </summary>
    /// <returns>A list of complex entity models.</returns>
    [HttpGet("complex")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntitiesAsync()
    {
        var data = await repository.GetAllComplexAsync();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves a single complex entity representation by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the complex entity.</param>
    /// <returns>The requested complex entity model.</returns>
    /// <response code="200">Returns the requested complex entity.</response>
    /// <response code="404">If the complex entity with the specified identifier was not found.</response>
    [HttpGet("complex/{id}")]
    public async Task<ActionResult<TComplex>> GetComplexEntityAsync(string id)
    {
        var data = await repository.GetComplexByIdAsync(id);
        if (data is null) return NotFound();
        return Ok(data);
    }
}