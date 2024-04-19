using Identity.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;

namespace Identity.Api.Controllers
{
    [Route("api/v1/identity/[Controller]")]
    public class EmailsController : BaseController
    {
        public EmailsController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterEmailUserCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPatch("{email}/activation-key/{key}")]
        public async Task<IActionResult> ActivateEmailUser([FromRoute] string email, [FromRoute] string key)
        {
            var command = new ActivateEmailUserCommand() { Email = email, VerificationKey = key };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("{email}/activation-key")]
        public async Task<IActionResult> ActivationKey([FromRoute] string email)
        {
            var command = new ActivateEmailUserRequestCommand() { Email = email };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("{email}/verification-key")]
        public async Task<IActionResult> VerificationKey([FromRoute] string email)
        {
            var command = new EmailUserVerificationKeyCommand() { Email = email };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPatch("{email}/verification-key/{key}")]
        public async Task<IActionResult> VerificationKey([FromRoute] string email, [FromRoute] string key)
        {
            var command = new VerifyEmailUserCommand() { Email = email, VerificationKey = key };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

    }
}
