using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Application.Features.Users.Queries.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
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
    public class GetUserRolesQuery : FluentResultRequest<IEnumerable<UserRoleViewModel>>
    {
        public Guid Id { get; set; }

        public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<IEnumerable<UserRoleViewModel>>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetUserRolesQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<IEnumerable<UserRoleViewModel>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<IEnumerable<UserRoleViewModel>>();


                var foundUser = await _unitOfWork.Users.FindByIdAsync(request.Id);

                var roles = foundUser?.Roles.Adapt<IEnumerable<UserRoleViewModel>>();

                if (roles == null)
                    roles = new List<UserRoleViewModel>();

                result.WithValue(roles);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
