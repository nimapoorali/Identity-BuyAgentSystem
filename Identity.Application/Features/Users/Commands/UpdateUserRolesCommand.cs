using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Permissions;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Mapster;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands
{
    public class UpdateUserRolesCommand : FluentResultRequest<UserRolesViewModel>
    {
        public Guid? UserId { get; set; }
        public Guid[]? RoleIds { get; set; }

        public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, Result<UserRolesViewModel>>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public UpdateUserRolesCommandHandler(IIdentityUnitOfWork unitOfWork, IPermissionService permissionService)
            {
                IdentityUnitOfWork = unitOfWork;
                PermissionService = permissionService;
            }

            public IPermissionService PermissionService { get; }

            public async Task<Result<UserRolesViewModel>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<UserRolesViewModel>();

                try
                {
                    var user = await IdentityUnitOfWork.Users.FindByIdAsync(request.UserId!.Value, cancellationToken);

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.InvalidRecord));

                    var removedRoleIds = user.Roles.Where(ur => !request.RoleIds!.Contains(ur.RoleId)).Select(ur => ur.RoleId).ToList();

                    foreach (var item in removedRoleIds)
                    {
                        var role = user.Roles.First(ur => ur.RoleId == item);
                        user.RemoveRole(role);
                    }

                    var addedRoleIds = request.RoleIds!.Where(r => !user.Roles.Select(ur => ur.RoleId).Contains(r)).ToList();
                    foreach (var item in addedRoleIds)
                    {
                        var userRole = UserRole.Create(request.UserId.Value, item, ActivityState.Active);
                        user.AddRole(userRole);
                    }

                    IdentityUnitOfWork.Users.Update(user);

                    await IdentityUnitOfWork.SaveChangesAsync();

                    PermissionService.UserPermissionsChanged(user.Id);

                    var roles = user.Roles.Adapt<UserRolesViewModel>();

                    result.WithValue(roles);
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
