using Identity.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;

namespace Identity.Api.Controllers
{
    [Route("api/v1/identity/[Controller]")]
    public class MobilesController : BaseController
    {
        public MobilesController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterMobileUserCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPatch("{mobile}/activation-key/{key}")]
        public async Task<IActionResult> ActivateMobileUser([FromRoute] string mobile, [FromRoute] string key)
        {
            var command = new ActivateMobileUserCommand() { Mobile = mobile, VerificationKey = key };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("{mobile}/activation-key")]
        public async Task<IActionResult> ActivationKey([FromRoute] string mobile)
        {
            var command = new ActivateMobileUserRequestCommand() { Mobile = mobile };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPost("{mobile}/verification-key")]
        public async Task<IActionResult> VerificationKey([FromRoute] string mobile)
        {
            var command = new MobileUserVerificationKeyCommand() { Mobile = mobile };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpPatch("{mobile}/verification-key/{key}")]
        public async Task<IActionResult> VerificationKey([FromRoute] string mobile, [FromRoute] string key)
        {
            var command = new VerifyMobileUserCommand() { Mobile = mobile, VerificationKey = key };

            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);
        }

    }
}
