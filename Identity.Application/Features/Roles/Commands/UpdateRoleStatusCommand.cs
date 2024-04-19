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

namespace Identity.Application.Features.Roles.Commands
{
    public class UpdateRoleStatusCommand : FluentResultRequest
    {
        public Guid? Id { get; set; }
        public int? ActivityStatus { get; set; }

        public class UpdateRoleStatusCommandHandler : IRequestHandler<UpdateRoleStatusCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdateRoleStatusCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpdateRoleStatusCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var activityState = Enumeration.FromValue<ActivityState>(request.ActivityStatus);

                    if (activityState is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(string.Format(Validations.InvalidValueForField, IdentityDataDictionary.ActivityState)));

                    var role = await UnitOfWork.Roles.FindByIdAsync(request.Id!.Value);

                    if (role is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    if (activityState == ActivityState.Active)
                        role.Activate();

                    if (activityState == ActivityState.Deactive)
                        role.Deactivate();

                    if (activityState == ActivityState.Suspend)
                        role.Suspend();

                    UnitOfWork.Roles.Update(role);
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
