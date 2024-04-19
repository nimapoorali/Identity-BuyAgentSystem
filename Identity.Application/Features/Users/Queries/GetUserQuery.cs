using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Queries.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
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

namespace Identity.Application.Features.Users.Queries
{
    public class GetUserQuery : FluentResultRequest<UserViewModel?>
    {
        public Guid Id { get; set; }

        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserViewModel?>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetUserQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<UserViewModel?>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<UserViewModel?>();


                try
                {
                    var foundUser = await _unitOfWork.Users.FindByIdAsync(request.Id);

                    var user = foundUser?.Adapt<UserViewModel>();

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    result.WithValue(user);
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
