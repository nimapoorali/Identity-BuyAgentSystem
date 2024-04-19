using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/v1/identity/[Controller]")]
    public class SecuredController : CheckPermissionController
    {

        [HttpGet("data")]
        public async Task<IActionResult> Data()
        {
            return Ok("This secured data is available only for authenticated users in service Identity.");
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("special-data")]
        [CheckPermission(PermissionName = "secured-data")]
        public async Task<IActionResult> SpecialData()
        {
            return Ok("This secured data is available only for special authenticated users in service Identity.");
        }
    }
}
