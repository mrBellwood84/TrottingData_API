using API.Controllers.Base;
using Application.DatasetBuilder.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Datasets;

namespace API.Controllers.Datasets;

public class RaceCardController(IDatasetBuilderService<DatasetBasic> builder) : DatasetController
{
    
    [HttpGet("{raceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DatasetBasic>> GetDatasetAsync([FromRoute] string raceId)
    {
        var data = await builder.BuildAsync(raceId);
        return Ok(data);
    }
}