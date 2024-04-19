using Identity.Application.Features.Roles.Commands;
using Identity.Application.Features.Roles.Queries;
using Identity.Application.Features.Users.Commands;
using Identity.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;

namespace Identity.Api.Controllers
{
    [Route("api/v1/identity/[Controller]")]
    public class RolesController : BaseController
    {
        public RolesController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewRoleCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Put([FromRoute] Guid? id, [FromBody] UpdateRoleCommand command)
        {
            if (command is null || id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.Id)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.RoleId));

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/activate")]
        [HttpPatch]
        public async Task<IActionResult> Activate([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            var command = new UpdateRoleStatusCommand()
            {
                Id = id,
                ActivityStatus = 1
            };

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/deactivate")]
        [HttpPatch]
        public async Task<IActionResult> Deactivate([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            var command = new UpdateRoleStatusCommand()
            {
                Id = id,
                ActivityStatus = 2
            };

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/suspend")]
        [HttpPatch]
        public async Task<IActionResult> Suspend([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            var command = new UpdateRoleStatusCommand()
            {
                Id = id,
                ActivityStatus = 3
            };

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var result = await Mediator.Send(new DeleteRoleCommand() { Id = id.Value });

            return Result(result);

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new GetAllRolesQuery());

            return Result(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var result = await Mediator.Send(new GetRoleQuery() { Id = id.Value });

            return Result(result);

        }
    }
}
