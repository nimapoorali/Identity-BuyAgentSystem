using Identity.Application.Features.Permissions.Commands;
using Identity.Application.Features.Permissions.Queries;
using Identity.Application.Features.Users.Commands;
using Identity.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;

namespace Identity.Api.Controllers
{
    [Route("api/v1/identity/[Controller]")]
    public class PermissionsController : BaseController
    {
        public PermissionsController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewPermissionCommand command)
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
        public async Task<IActionResult> Put([FromRoute] Guid? id, [FromBody] UpdatePermissionCommand command)
        {
            if (command is null || id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.Id)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.PermissionId));

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

            var command = new UpdatePermissionStatusCommand()
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

            var command = new UpdatePermissionStatusCommand()
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

            var command = new UpdatePermissionStatusCommand()
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

            var result = await Mediator.Send(new DeletePermissionCommand() { Id = id.Value });

            return Result(result);

        }

        [Route("{id}/roles")]
        [HttpGet]
        public async Task<IActionResult> GetPermissionRoles([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var command = new GetPermissionRolesQuery()
            {
                Id = id
            };

            var result = await Mediator.Send(command);

            return Result(result);
        }

        [Route("{id}/roles")]
        [HttpPut]
        public async Task<IActionResult> SetRoles([FromRoute] Guid id, [FromBody] Guid[] roleIds)
        {
            if (id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            if (roleIds is null || roleIds.Length == 0)
                return BadResult(IdentityValidations.NoRoleDefinedForPermission);

            var command = new UpdatePermissionRolesCommand()
            {
                PermissionId = id,
                RoleIds = roleIds,
            };

            var result = await Mediator.Send(command);

            return Result(result);
        }

        [Route("{id}/roles")]
        [HttpDelete]
        public async Task<IActionResult> ClearRoles([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var command = new ClearPermissionRolesCommand()
            {
                PermissionId = id
            };

            var result = await Mediator.Send(command);

            return Result(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new GetAllPermissionsQuery());

            return Result(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var result = await Mediator.Send(new GetPermissionQuery() { Id = id.Value });

            return Result(result);

        }
    }
}
