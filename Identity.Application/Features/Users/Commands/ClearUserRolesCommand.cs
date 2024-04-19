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
    public class ClearUserRolesCommand : FluentResultRequest
    {
        public Guid? UserId { get; set; }

        public class ClearUserRolesCommandHandler : IRequestHandler<ClearUserRolesCommand, Result>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public ClearUserRolesCommandHandler(IIdentityUnitOfWork unitOfWork, IPermissionService permissionService)
            {
                IdentityUnitOfWork = unitOfWork;
                PermissionService = permissionService;
            }

            public IPermissionService PermissionService { get; }

            public async Task<Result> Handle(ClearUserRolesCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var user = await IdentityUnitOfWork.Users.FindByIdAsync(request.UserId!.Value, cancellationToken);

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.InvalidRecord));

                    var removedRoleIds = user.Roles.Select(ur => ur.RoleId).ToList();

                    foreach (var item in removedRoleIds)
                    {
                        var role = user.Roles.First(ur => ur.RoleId == item);
                        user.RemoveRole(role);
                    }

                    IdentityUnitOfWork.Users.Update(user);

                    await IdentityUnitOfWork.SaveChangesAsync();

                    PermissionService.UserPermissionsChanged(user.Id);

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
