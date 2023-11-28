using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Permissions.Commands.ViewModels;
using Identity.Application.Services;
using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.SeedWork;
using Identity.Domain.Models.SharedKernel;
using Identity.Domain.Models.SharedKernel.Rules;
using Mapster;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Commands
{
    public class UpdatePermissionRolesCommand : FluentResultRequest<PermissionRolesViewModel>
    {
        public Guid? PermissionId { get; set; }
        public Guid[]? RoleIds { get; set; }

        public class UpdatePermissionRolesCommandHandler : IRequestHandler<UpdatePermissionRolesCommand, Result<PermissionRolesViewModel>>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public UpdatePermissionRolesCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                IdentityUnitOfWork = unitOfWork;
            }

            public async Task<Result<PermissionRolesViewModel>> Handle(UpdatePermissionRolesCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<PermissionRolesViewModel>();

                try
                {
                    var permission = await IdentityUnitOfWork.Permissions.FindByIdAsync(request.PermissionId!.Value, cancellationToken);

                    if (permission is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.InvalidRecord));

                    var removedRoleIds = permission.Roles
                        .Where(ur => !request.RoleIds!.Contains(ur.RoleId))
                        .Select(ur => ur.RoleId)
                        .ToList();

                    foreach (var item in removedRoleIds)
                    {
                        var role = permission.Roles.First(ur => ur.RoleId == item);
                        permission.RemoveRole(role);
                    }

                    var addedRoleIds = request.RoleIds!
                        .Where(r => !permission.Roles.Select(ur => ur.RoleId).Contains(r))
                        .ToList();
                    foreach (var item in addedRoleIds)
                    {
                        var permissionRole = PermissionRole.Create(item, request.PermissionId.Value, ActivityState.Active);
                        permission.AddRole(permissionRole);
                    }

                    IdentityUnitOfWork.Permissions.Update(permission);

                    await IdentityUnitOfWork.SaveChangesAsync();

                    //ToDo: Reset permissions cache for users has this permission 
                    //PermissionService.UserPermissionsChanged(user.Id);

                    var permissions = permission.Roles.Adapt<PermissionRolesViewModel>();

                    result.WithValue(permissions);
                    result.WithSuccess(Messages.OperationSucceeded);

                    return result;
                }
                catch (BusinessRuleValidationException ex)
                {
                    result.WithError(ex.Message);
                    return result.ToResult();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }
    }
}
