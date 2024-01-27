using Identity.Application.Features.Users.Queries;
using Identity.Application.Features.Users.Commands;
using Identity.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;
using Identity.Application.Features.Roles.Commands;

namespace Identity.Api.Controllers
{
    [Route("api/v1/identity/[Controller]")]
    public class UsersController : CheckPermissionBaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewUserCommand command)
        {
            if (command is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> Edit([FromRoute] Guid? id, [FromBody] UpdateUserCommand command)
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

        [Route("{id}/mobile")]
        [HttpPatch]
        public async Task<IActionResult> EditMobile([FromRoute] Guid? id, [FromBody] UpdateMobileCommand command)
        {
            if (command is null || id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.UserId)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.UserId));

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/email")]
        [HttpPatch]
        public async Task<IActionResult> EditEmail([FromRoute] Guid? id, [FromBody] UpdateEmailCommand command)
        {
            if (command is null || id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.UserId)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.UserId));

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/mobile")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMobile([FromRoute] Guid? id, [FromBody] DeleteMobileCommand command)
        {
            if (command is null || id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.UserId)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.UserId));

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/email")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmail([FromRoute] Guid? id, [FromBody] DeleteEmailCommand command)
        {
            if (command is null || id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.UserId)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.UserId));

            var result = await Mediator.Send(command);

            return Result(result);

        }


        [Route("{id}/mobile/verification-key")]
        [HttpPost]
        public async Task<IActionResult> VerificationKeyForMobile([FromRoute] Guid? id)
        {
            if (id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            MobileVerificationKeyCommand command = new() { UserId = id };

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/email/verification-key")]
        [HttpPost]
        public async Task<IActionResult> VerificationKeyForEmail([FromRoute] Guid? id)
        {
            if (id is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            EmailVerificationKeyCommand command = new() { UserId = id };

            var result = await Mediator.Send(command);

            return Result(result);

        }


        [Route("{id}/mobile/verification-key/{key}")]
        [HttpPatch]
        public async Task<IActionResult> VerifyMobile([FromRoute] Guid? id, [FromRoute] string key, [FromBody] VerifyMobileCommand command)
        {
            if (command is null || id is null || key is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.UserId)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.UserId));
            if (key != command.VerificationKey)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, SharedDataDictionary.MobileVerificationKey));

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [Route("{id}/email/verification-key/{key}")]
        [HttpPatch]
        public async Task<IActionResult> VerifyEmail([FromRoute] Guid? id, [FromRoute] string key, [FromBody] VerifyEmailCommand command)
        {
            if (command is null || id is null || key is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());

            if (id != command.UserId)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, IdentityDataDictionary.UserId));
            if (key != command.VerificationKey)
                return BadResult(string.Format(Validations.ValuesNotMatchForField, SharedDataDictionary.EmailVerificationKey));

            var result = await Mediator.Send(command);

            return Result(result);

        }

        
        [Route("{id}/roles")]
        [HttpGet]
        public async Task<IActionResult> GetUserRoles([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var command = new GetUserRolesQuery()
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
                return BadResult(IdentityValidations.NoRoleDefinedForUser);

            var command = new UpdateUserRolesCommand()
            {
                UserId = id,
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

            var command = new ClearUserRolesCommand()
            {
                UserId = id
            };

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

            var command = new UpdateUserStatusCommand()
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

            var command = new UpdateUserStatusCommand()
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

            var command = new UpdateUserStatusCommand()
            {
                Id = id,
                ActivityStatus = 3
            };

            var result = await Mediator.Send(command);

            return Result(result);

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new GetAllUsersQuery());

            return Result(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var result = await Mediator.Send(new GetUserQuery() { Id = id.Value });

            return Result(result);

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return BadResult(Validations.InvalidInputData);

            var result = await Mediator.Send(new DeleteUserCommand() { Id = id.Value });

            return Result(result);

        }
    }
}
