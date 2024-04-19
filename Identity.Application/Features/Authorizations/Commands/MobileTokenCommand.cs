using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Users;
using Identity.Application.Features.Authorizations.Commands.ViewModels;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
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

namespace Identity.Application.Features.Authorizations.Commands
{
    public class MobileTokenCommand : FluentResultRequest<TokenViewModel>
    {
        public string? Mobile { get; set; }
        public string? VerificationKey { get; set; }

        public class MobileTokenCommandHandler : IRequestHandler<MobileTokenCommand, Result<TokenViewModel>>
        {
            private readonly ITokenService TokenService;
            public IIdentityUnitOfWork UnitOfWork { get; }

            public MobileTokenCommandHandler(IIdentityUnitOfWork unitOfWork, ITokenService tokenService)
            {
                UnitOfWork = unitOfWork;
                TokenService = tokenService;
            }

            public async Task<Result<TokenViewModel>> Handle(MobileTokenCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<TokenViewModel>();

                try
                {
                    if (request.VerificationKey is null)
                        throw new BusinessRuleValidationException(
                            string.Format(Validations.RequiredField, SharedDataDictionary.MobileVerificationKey));

                    if (request.Mobile is null)
                        throw new BusinessRuleValidationException(
                            string.Format(Validations.RequiredField, SharedDataDictionary.Mobile));


                    var username = Username.Create(request.Mobile);

                    var mobileUser = await UnitOfWork.Users.SingleAsync(u => u.Username == username);

                    if (mobileUser is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!mobileUser.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (mobileUser.Mobile is null)
                        throw new BusinessRuleValidationException(IdentityValidations.NoMobileSet);

                    if (!mobileUser.Mobile.IsVerified)
                        throw new BusinessRuleValidationException(IdentityValidations.MobileNotVerified);

                    if (mobileUser.Mobile.KeyExpirationDate is not null && mobileUser.Mobile.KeyExpirationDate < DateTimeP.Now)
                        throw new BusinessRuleValidationException(Validations.ExpiredVerificationKey);

                    if (mobileUser.Mobile.VerificationKey != request.VerificationKey)
                        throw new BusinessRuleValidationException(Validations.InvalidVerificationKey);

                    var generatedToken = await TokenService.GenerateToken(mobileUser.Id);

                    var token = generatedToken.Adapt<TokenViewModel>();

                    result.WithValue(token);
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
