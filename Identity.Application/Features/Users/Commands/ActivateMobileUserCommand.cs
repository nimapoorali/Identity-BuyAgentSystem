using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using NP.Shared.Domain.Models.SeedWork;
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
    public class ActivateMobileUserCommand : FluentResultRequest<NewUserViewModel>
    {
        //public string? Username { get; set; }
        public string? Mobile { get; set; }
        public string? VerificationKey { get; set; }

        public class ActivateUserByMobileCommandHandler : IRequestHandler<ActivateMobileUserCommand, Result<NewUserViewModel>>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public ActivateUserByMobileCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                IdentityUnitOfWork = unitOfWork;
            }

            public async Task<Result<NewUserViewModel>> Handle(ActivateMobileUserCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewUserViewModel>();

                try
                {
                    var username = Domain.Models.Aggregates.Users.ValueObjects.Username.Create(request.Mobile!);

                    var mobileUser = await IdentityUnitOfWork.Users.SingleAsync(u => u.Username == username);

                    if (mobileUser is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    mobileUser.ActivateByMobile(request.VerificationKey);

                    IdentityUnitOfWork.Users.Update(mobileUser);
                    await IdentityUnitOfWork.SaveChangesAsync();

                    var activateUser = mobileUser.Adapt<NewUserViewModel>();

                    result.WithValue(activateUser);
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
