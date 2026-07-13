using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Persistence.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("/model/[controller]")]
public class ModelController<TSimple, TComplex>(IDbService<TSimple, TComplex> dbService, EntityPolicy<TSimple> policy) : ControllerBase
{
    [HttpGet("id")]
    public async Task<ActionResult<List<IdEntity>>> GetIdsAsync()
    {
        var data = await dbService.GetIdsAsync();
        return Ok(data);
    }

    [HttpGet("entity")]
    public async Task<ActionResult<List<TSimple>>> GetEntitiesAsync()
    {
        if (!policy.AllowAllSimple)
            return StatusCode(StatusCodes.Status405MethodNotAllowed, 
                $"Bulk retrieval of {typeof(TSimple).Name} is disabled due to dataset size.");
        
        var data = await dbService.GetAllSimpleAsync();
        return Ok(data);
    }

    [HttpGet("entity/{id}")]
    public async Task<ActionResult<TSimple>> GetEntityAsync(string id)
    {
        var data = await dbService.GetSimpleByIdAsync(id);
        if  (data == null) return NotFound();
        return Ok(data);
    }

    [HttpGet("complex")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntitiesAsync()
    {
        if (!policy.AllowAllComplex)
            return StatusCode(StatusCodes.Status405MethodNotAllowed, 
                $"Bulk retrieval of complex {typeof(TComplex).Name} is disabled due to dataset size.");
        
        var data = await dbService.GetAllComplexAsync();
        return Ok(data);
    }

    [HttpGet("complex/{id}")]
    public async Task<ActionResult<List<TComplex>>> GetComplexEntityAsync(string id)
    {
        var data = await dbService.GetComplexByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }
}