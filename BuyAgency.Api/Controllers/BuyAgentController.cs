using Microsoft.AspNetCore.Mvc;

namespace BuyAgency.Api.Controllers
{
    [Route("api/v1/buyagent/[Controller]")]
    public class BuyAgentController : Controller
    {
        [HttpGet]
        public IActionResult Agents()
        {
            return new JsonResult("Agents list...");
        }
    }
}
