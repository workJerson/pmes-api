using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace helply_api.Common
{
    [ApiExplorerSettings(GroupName = "v1")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class VersionedController : BaseApiController
    {
    }
}
