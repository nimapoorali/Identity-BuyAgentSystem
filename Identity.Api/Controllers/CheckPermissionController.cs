using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Authorize]//Check for authentication before autherization
    [CheckPermission(Order = 1)]
    public abstract class CheckPermissionController : Controller
    {

    }
}
