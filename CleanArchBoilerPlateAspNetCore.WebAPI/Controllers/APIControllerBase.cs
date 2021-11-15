using Microsoft.AspNetCore.Mvc;

namespace CleanArchBoilerPlateAspNetCore.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    //[Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    public abstract class APIControllerBase : ControllerBase
    {

    }
}
