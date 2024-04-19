using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Common;

namespace NP.Shared.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IMediator Mediator { get; }

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected ActionResult Result<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.ToNPResult());
            }
            else
            {
                return BadRequest(result.ToResult().ToNPResult());
            }
        }

        protected ActionResult Result(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.ToNPResult());
            }
            else
            {
                return BadRequest(result.ToNPResult());
            }
        }

        protected ActionResult BadResult(params string[] messages)
        {
            return Result(new Result().WithErrors(messages));
        }

    }
}
