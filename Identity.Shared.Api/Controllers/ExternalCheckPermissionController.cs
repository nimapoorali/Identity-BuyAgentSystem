using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Shared.Api.Controllers
{
    //[Authorize]
    [ExternalCheckPermission(Order = 1)]
    public abstract class ExternalCheckPermissionController : Controller
    {

    }
}
