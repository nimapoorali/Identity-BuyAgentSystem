using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
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

namespace Identity.Application.Features.Roles.Commands
{
    public class DeleteRoleCommand : FluentResultRequest<Guid>
    {
        public Guid Id { get; set; }

        public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result<Guid>>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public DeleteRoleCommandHandler(IIdentityUnitOfWork identityUnitOfWork)
            {
                IdentityUnitOfWork = identityUnitOfWork;
            }

            public async Task<Result<Guid>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<Guid>();

                try
                {
                    var role = await IdentityUnitOfWork.Roles.FindByIdAsync(request.Id);
                    if (role is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    IdentityUnitOfWork.Roles.Remove(role);
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
