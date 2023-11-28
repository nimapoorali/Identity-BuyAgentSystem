using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Permissions;
using Identity.Application.Features.Users.Commands;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
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
    public class ClearPermissionRolesCommand : FluentResultRequest
    {
        public Guid? PermissionId { get; set; }

        public class ClearPermissionRolesCommandHandler : IRequestHandler<ClearPermissionRolesCommand, Result>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public ClearPermissionRolesCommandHandler(IIdentityUnitOfWork unitOfWork, IPermissionService permissionService)
            {
                IdentityUnitOfWork = unitOfWork;
                PermissionService = permissionService;
            }

            public IPermissionService PermissionService { get; }

            public async Task<Result> Handle(ClearPermissionRolesCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var permission = await IdentityUnitOfWork.Permissions.FindByIdAsync(request.PermissionId!.Value, cancellationToken);

                    if (permission is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.InvalidRecord));

                    var removedRoleIds = permission.Roles.Select(ur => ur.RoleId).ToList();

                    foreach (var item in removedRoleIds)
                    {
                        var role = permission.Roles.First(ur => ur.RoleId == item);
                        permission.RemoveRole(role);
                    }

                    IdentityUnitOfWork.Permissions.Update(permission);

                    await IdentityUnitOfWork.SaveChangesAsync();

                    //ToDo: Reset permissions cache for users has this permission 
                    //PermissionService.UserPermissionsChanged(user.Id);

                    result.WithSuccess(Messages.OperationSucceeded);

                    return result;
                }
                catch (BusinessRuleValidationException ex)
                {
                    result.WithError(ex.Message);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }
    }
}
