using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
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
    public class UpdatePermissionStatusCommand : FluentResultRequest
    {
        public Guid? Id { get; set; }
        public int? ActivityStatus { get; set; }

        public class UpdatePermissionStatusCommandHandler : IRequestHandler<UpdatePermissionStatusCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdatePermissionStatusCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpdatePermissionStatusCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var activityState = Enumeration.FromValue<ActivityState>(request.ActivityStatus);

                    if (activityState is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(string.Format(Validations.InvalidValueForField, IdentityDataDictionary.ActivityState)));

                    var permission = await UnitOfWork.Permissions.FindByIdAsync(request.Id!.Value);

                    if (permission is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    if (activityState == ActivityState.Active)
                        permission.Activate();

                    if (activityState == ActivityState.Deactive)
                        permission.Deactivate();

                    if (activityState == ActivityState.Suspend)
                        permission.Suspend();

                    UnitOfWork.Permissions.Update(permission);
                    await UnitOfWork.SaveChangesAsync();

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
