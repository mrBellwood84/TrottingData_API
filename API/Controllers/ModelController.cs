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
[Route("model/[controller]")]
[Produces("application/json")] // Forteller Scalar at alle endepunkter returnerer JSON
public abstract class ModelController<TSimple, TComplex>(IRepositoryService<TSimple, TComplex> repository)
    : ControllerBase 
    where TSimple : IDbItem 
    where TComplex : IDbItem
{
    /// <summary>
    /// Retrieves a list of all available unique identifiers (IDs) for this entity type.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> containing a list of <see cref="IdModel"/>s.</returns>
    /// <response code="200">Successfully retrieved the list of IDs.</response>
    /// <response code="403">If the model policy restricts access to retrieving IDs for this entity type.</response>
    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<IdModel>>> GetIdsAsync()
    {
        var data = await repository.GetIdsAsync();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves all simple representations of the entity.
    /// </summary>
    /// <returns>A list of simple entity models.</returns>
    /// <response code="200">Successfully retrieved all simple entities.</response>
    /// <response code="403">If the model policy restricts bulk retrieval of simple entities.</response>
    [HttpGet("entity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// <response code="200">Successfully retrieved the requested entity.</response>
    /// <response code="403">If the model policy restricts access to this specific simple entity.</response>
    /// <response code="404">If the entity with the specified identifier was not found.</response>
    [HttpGet("entity/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// <response code="200">Successfully retrieved all complex entities.</response>
    /// <response code="403">If the model policy restricts bulk retrieval of complex entities.</response>
    [HttpGet("complex")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// <response code="200">Successfully retrieved the requested complex entity.</response>
    /// <response code="403">If the model policy restricts access to this specific complex entity.</response>
    /// <response code="404">If the complex entity with the specified identifier was not found.</response>
    [HttpGet("complex/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TComplex>> GetComplexEntityAsync(string id)
    {
        var data = await repository.GetComplexByIdAsync(id);
        if (data is null) return NotFound();
        return Ok(data);
    }
}