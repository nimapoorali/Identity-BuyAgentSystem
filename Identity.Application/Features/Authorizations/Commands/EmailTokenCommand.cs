using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Users;
using Identity.Application.Features.Authorizations.Commands.ViewModels;
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
    public class EmailTokenCommand : FluentResultRequest<TokenViewModel>
    {
        public string? Email { get; set; }
        public string? VerificationKey { get; set; }

        public class EmailTokenCommandHandler : IRequestHandler<EmailTokenCommand, Result<TokenViewModel>>
        {
            private readonly ITokenService TokenService;
            public IIdentityUnitOfWork UnitOfWork { get; }

            public EmailTokenCommandHandler(IIdentityUnitOfWork identityUnitOfWork, ITokenService tokenService)
            {
                UnitOfWork = identityUnitOfWork;
                TokenService = tokenService;
            }

            public async Task<Result<TokenViewModel>> Handle(EmailTokenCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<TokenViewModel>();

                try
                {
                    if (request.Email is null)
                        throw new BusinessRuleValidationException(string.Format(Validations.RequiredField, SharedDataDictionary.Email));

                    var emailUsernameObject = Domain.Models.Aggregates.Users.ValueObjects
                       .Username.Create(request.Email!);

                    var emailUser = await UnitOfWork.Users.SingleAsync(user => user.Username == emailUsernameObject);

                    if (emailUser is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!emailUser.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (emailUser.Email is null )
                        throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

                    if (!emailUser.Email.IsVerified)
                        throw new BusinessRuleValidationException(IdentityValidations.EmailNotVerified);

                    if (emailUser.Email.KeyExpirationDate is not null && emailUser.Email.KeyExpirationDate < DateTimeP.Now)
                        throw new BusinessRuleValidationException(Validations.ExpiredVerificationKey);

                    if (emailUser.Email.VerificationKey != request.VerificationKey)
                        throw new BusinessRuleValidationException(Validations.InvalidVerificationKey);
                    
                    
                    var generatedToken = await TokenService.GenerateToken(emailUser.Id);

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
