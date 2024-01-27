using FluentResults;
using Identity.Application.Abstraction;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using Identity.Resources;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Authorizations.Commands
{
    public class EmailTokenRequestCommand : FluentResultRequest
    {
        public string? Email { get; set; }

        public class EmailTokenRequestCommandHandler : IRequestHandler<EmailTokenRequestCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public EmailTokenRequestCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }


            public async Task<Result> Handle(EmailTokenRequestCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    if (request.Email is null)
                        throw new BusinessRuleValidationException(string.Format(Validations.RequiredField, SharedDataDictionary.Email));

                    var emailUsernameObject = Domain.Models.Aggregates.Users.ValueObjects
                       .Username.Create(request.Email);

                    var emailUser = await UnitOfWork.Users.SingleAsync(user => user.Username == emailUsernameObject);

                    if (emailUser is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!emailUser.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (emailUser.Email is null)
                        throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

                    if (!emailUser.Email.IsVerified)
                        throw new BusinessRuleValidationException(IdentityValidations.EmailNotVerified);

                    var verificationKey = Guid.NewGuid().ToString();
                    var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(10));
                    var email = NP.Shared.Domain.Models.SharedKernel
                        .Email.Create(emailUser.Email!.Value, emailUser.Email!.IsVerified, verificationKey, keyExpirationDate);

                    emailUser.ChangeEmail(email);

                    UnitOfWork.Users.Update(emailUser);
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
