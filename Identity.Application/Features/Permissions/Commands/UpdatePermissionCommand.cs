using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Permissions.Commands.ViewModels;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
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

namespace Identity.Application.Features.Permissions.Commands
{
    public class UpdatePermissionCommand : FluentResultRequest<NewPermissionViewModel>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? ActivityState { get; set; }

        public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Result<NewPermissionViewModel>>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdatePermissionCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result<NewPermissionViewModel>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewPermissionViewModel>();

                try
                {
                    var name = PermissionName.Create(request.Name!);


                    var permission = await UnitOfWork.Permissions.FindByIdAsync(request.Id!.Value);

                    if (permission is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    var activityState = request.ActivityState is null ? NP.Shared.Domain.Models.SharedKernel.ActivityState.Deactive :
                          Enumeration.FromName<ActivityState>(request.ActivityState);

                    permission.Change(name, activityState);

                    UnitOfWork.Permissions.Update(permission);
                    await UnitOfWork.SaveChangesAsync();

                    var roleVieModel = permission.Adapt<NewPermissionViewModel>();

                    result.WithValue(roleVieModel);
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
