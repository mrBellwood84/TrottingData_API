using API.Controllers.Base;
using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Complex;
using Models.Entity;

namespace API.Controllers.Models;

public class RaceResultController(
    IRaceResultRepository repository)
    : ReadSingleModelController<RaceResultEntity, RaceResultsComplex>(repository)
{
    [HttpGet("entity/participant/{participantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RaceResultEntity>> GetResultEntityFromParticipantAsync(
        [FromRoute] string participantId)
    {
        var data = await repository.GetEntityByParticipantIdAsync(participantId);
        if (data is null) return NotFound();
        return Ok(data);
    }

    [HttpGet("complex/participant/{participantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RaceResultsComplex>> GetResultComplexByParticipantAsync(
        [FromRoute] string participantId)
    {
        var data = await repository.GetComplexByParticipantIdAsync(participantId);
        if (data is null) return NotFound();
        return Ok(data);
    }
}