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

namespace Identity.Application.Features.Users.Commands
{
    public class UpdateUserStatusCommand : FluentResultRequest
    {
        public Guid? Id { get; set; }
        public int? ActivityStatus { get; set; }

        public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdateUserStatusCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var activityState = Enumeration.FromValue<ActivityState>(request.ActivityStatus);

                    if (activityState is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(string.Format(Validations.InvalidValueForField, IdentityDataDictionary.ActivityState)));

                    var user = await UnitOfWork.Users.FindByIdAsync(request.Id!.Value);

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    if (activityState == ActivityState.Active)
                        user.Activate();

                    if (activityState == ActivityState.Deactive)
                        user.Deactivate();

                    if (activityState == ActivityState.Suspend)
                        user.Suspend();

                    UnitOfWork.Users.Update(user);
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
