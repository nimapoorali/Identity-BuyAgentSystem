using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using Identity.Resources;
using Mapster;
using MediatR;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands
{
    public class ActivateMobileUserRequestCommand : FluentResultRequest
    {
        public string? Mobile { get; set; }

        public class ActivateMobileUserRequestCommandHandler : IRequestHandler<ActivateMobileUserRequestCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public ActivateMobileUserRequestCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(ActivateMobileUserRequestCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    if (request.Mobile is null)
                        throw new BusinessRuleValidationException(string.Format(Validations.RequiredField, SharedDataDictionary.Mobile));

                    var mobileUsernameObject = Domain.Models.Aggregates.Users.ValueObjects
                       .Username.Create(request.Mobile);

                    var user = await UnitOfWork.Users.SingleAsync(user => user.Username == mobileUsernameObject);

                    if (user is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (user.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.UserAlreadyActivated);

                    if (user.Mobile is null)
                        throw new BusinessRuleValidationException(string.Format(Validations.AlreadyInState, IdentityDataDictionary.User));

                    var verificationKey = StringUtil.RandomNumeric(5);
                    var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(2));
                    var mobile = NP.Shared.Domain.Models.SharedKernel
                        .Mobile.Create(user.Mobile!.Value, user.Mobile!.IsVerified, verificationKey, keyExpirationDate);

                    user.ChangeMobile(mobile);

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
