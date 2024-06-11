using Microsoft.AspNetCore.Mvc;

namespace helply_api.Common
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
