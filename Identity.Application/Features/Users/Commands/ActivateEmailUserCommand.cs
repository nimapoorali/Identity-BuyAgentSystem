using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
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
    public class ActivateEmailUserCommand : FluentResultRequest<NewUserViewModel>
    {
        //public string? Username { get; set; }
        public string? Email { get; set; }
        public string? VerificationKey { get; set; }

        public class ActivateEmailUserCommandHandler : IRequestHandler<ActivateEmailUserCommand, Result<NewUserViewModel>>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public ActivateEmailUserCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                IdentityUnitOfWork = unitOfWork;
            }

            public async Task<Result<NewUserViewModel>> Handle(ActivateEmailUserCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewUserViewModel>();

                try
                {
                    var username = Domain.Models.Aggregates.Users.ValueObjects.Username.Create(request.Email!);

                    var emailUser = await IdentityUnitOfWork.Users.SingleAsync(u => u.Username == username);

                    if (emailUser is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    emailUser.ActivateByEmail(request.VerificationKey);

                    IdentityUnitOfWork.Users.Update(emailUser);
                    await IdentityUnitOfWork.SaveChangesAsync();

                    var activateUser = emailUser.Adapt<NewUserViewModel>();

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
