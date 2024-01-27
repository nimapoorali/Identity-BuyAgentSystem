using FluentResults;
using Identity.Application.Abstraction;
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

namespace Identity.Application.Features.Roles.Commands
{
    public class DeleteUserCommand : FluentResultRequest<Guid>
    {
        public Guid Id { get; set; }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<Guid>>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public DeleteUserCommandHandler(IIdentityUnitOfWork identityUnitOfWork)
            {
                IdentityUnitOfWork = identityUnitOfWork;
            }

            public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<Guid>();

                try
                {
                    var user = await IdentityUnitOfWork.Users.FindByIdAsync(request.Id);
                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    IdentityUnitOfWork.Users.Remove(user);
                    await IdentityUnitOfWork.SaveChangesAsync();

                    result.WithValue(request.Id);
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
