using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

[ApiController]
[Route("dataset/[controller]")]
[Produces("application/json")]
public class DatasetController : ControllerBase
{
}