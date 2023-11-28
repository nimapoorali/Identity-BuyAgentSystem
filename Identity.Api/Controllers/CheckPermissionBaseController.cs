using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NP.Shared.Api.Controllers;

namespace Identity.Api.Controllers
{
    [Authorize]
    [CheckPermission(Order = 1)]
    public abstract class CheckPermissionBaseController : BaseController
    {
        protected CheckPermissionBaseController(IMediator mediator) : base(mediator)
        {
        }
    }
}
