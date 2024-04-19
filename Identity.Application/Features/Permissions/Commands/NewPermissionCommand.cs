using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Permissions.Commands.ViewModels;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
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
    public class NewPermissionCommand : FluentResultRequest<NewPermissionViewModel>
    {
        public string? Name { get; set; }
        public string? ActivityState { get; set; }

        public class NewPermissionCommandHandler : IRequestHandler<NewPermissionCommand, Result<NewPermissionViewModel>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public NewPermissionCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<NewPermissionViewModel>> Handle(NewPermissionCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewPermissionViewModel>();

                try
                {
                    var name = PermissionName.Create(request.Name!);

                    var activityState = request.ActivityState is null ? NP.Shared.Domain.Models.SharedKernel.ActivityState.Deactive :
                           Enumeration.FromName<ActivityState>(request.ActivityState);

                    var permission = Permission.Create(name, activityState);

                    _unitOfWork.Permissions.AddAsync(permission);
                    await _unitOfWork.SaveChangesAsync();


                    var newPermission = permission.Adapt<NewPermissionViewModel>();

                    result.WithValue(newPermission);
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
