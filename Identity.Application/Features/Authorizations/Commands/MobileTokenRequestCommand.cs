using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Users;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using Identity.Resources;
using MediatR;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Authorizations.Commands
{
    public class MobileTokenRequestCommand : FluentResultRequest
    {
        public string? Mobile { get; set; }

        public class MobileTokenRequestCommandHandler : IRequestHandler<MobileTokenRequestCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public MobileTokenRequestCommandHandler( IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }


            public async Task<Result> Handle(MobileTokenRequestCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    if (request.Mobile is null)
                        throw new BusinessRuleValidationException(string.Format(Validations.RequiredField, SharedDataDictionary.Mobile));

                    var mobileUsernameObject = Domain.Models.Aggregates.Users.ValueObjects
                       .Username.Create(request.Mobile);

                    var mobileUser = await UnitOfWork.Users.SingleAsync(user => user.Username == mobileUsernameObject);

                    if (mobileUser is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!mobileUser.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (mobileUser.Mobile is null)
                        throw new BusinessRuleValidationException(IdentityValidations.NoMobileSet);

                    if (!mobileUser.Mobile.IsVerified)
                        throw new BusinessRuleValidationException(IdentityValidations.MobileNotVerified);

                    var verificationKey = StringUtil.RandomNumeric(5);
                    var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(2));
                    var mobile = NP.Shared.Domain.Models.SharedKernel
                        .Mobile.Create(mobileUser.Mobile.Value, mobileUser.Mobile.IsVerified, verificationKey, keyExpirationDate);

                    mobileUser.ChangeMobile(mobile);

                    UnitOfWork.Users.Update(mobileUser);
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
