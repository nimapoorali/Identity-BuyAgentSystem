using Identity.Application.Features.Authorizations.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;

namespace Identity.Api.Controllers
{

    [Route("api/v1/{appname}/[Controller]")]
    public class AuthController : BaseController
    {
        private const string APPNAME = "appname";

        public AuthController(IMediator mediator) : base(mediator) { }

        [HttpPost("token")]
        public async Task<IActionResult> Post([FromBody] TokenCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("mobile-tokens")]
        public async Task<IActionResult> Post([FromBody] MobileTokenRequestCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("email-tokens")]
        public async Task<IActionResult> Post([FromBody] EmailTokenRequestCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }


        [HttpPost("mobile-token")]
        public async Task<IActionResult> Post([FromBody] MobileTokenCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("email-token")]
        public async Task<IActionResult> Post([FromBody] EmailTokenCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

    }
}
