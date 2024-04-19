using Identity.Shared.Api;
using Identity.Shared.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BuyAgency.Api.Controllers
{
    [Route("api/v1/buyagent/[Controller]")]
    public class SecuredController : ExternalCheckPermissionController
    {
        [HttpGet("data")]
        public async Task<IActionResult> Data()
        {
            return Ok("This secured data is available only for authenticated users in service BuyAgent.");
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("special-data")]
        [ExternalCheckPermission(PermissionName = "secured-data")]
        public async Task<IActionResult> SpecialData()
        {
            return Ok("This secured data is available only for special authenticated users in service BuyAgent.");
        }
    }
}
