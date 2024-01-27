using FluentResults;
using Identity.Application.Abstraction;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
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
    public class EmailVerificationKeyCommand : FluentResultRequest
    {
        public Guid? UserId { get; set; }

        public class EmailVerificationKeyCommandHandler : IRequestHandler<EmailVerificationKeyCommand, Result>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public EmailVerificationKeyCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                IdentityUnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(EmailVerificationKeyCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    if (request.UserId is null)
                        throw new BusinessRuleValidationException(string.Format(Validations.RequiredField, IdentityDataDictionary.UserId));

                    var user = await IdentityUnitOfWork.Users.FindByIdAsync(request.UserId.Value);

                    if (user is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!user.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (user.Email is null)
                        throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

                    var verificationKey = Guid.NewGuid().ToString();
                    var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(10));
                    var email = NP.Shared.Domain.Models.SharedKernel
                        .Email.Create(user.Email!.Value, user.Email!.IsVerified, verificationKey, keyExpirationDate);

                    user.ChangeEmail(email);

                    IdentityUnitOfWork.Users.Update(user);
                    await IdentityUnitOfWork.SaveChangesAsync();
                    
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
