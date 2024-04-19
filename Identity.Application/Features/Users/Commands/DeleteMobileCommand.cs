using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
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
    public class DeleteMobileCommand : FluentResultRequest
    {
        public Guid? UserId { get; set; }

        public class DeleteMobileCommandHandler : IRequestHandler<DeleteMobileCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public DeleteMobileCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(DeleteMobileCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var user = await UnitOfWork.Users.FindByIdAsync(request.UserId!.Value);

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    user.DeleteMobile();

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
