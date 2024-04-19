using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Roles.Queries.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Mapster;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Roles.Queries
{
    public class GetAllRolesQuery : FluentResultRequest<IEnumerable<RoleViewModel>>
    {
        public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<IEnumerable<RoleViewModel>>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetAllRolesQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<IEnumerable<RoleViewModel>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<IEnumerable<RoleViewModel>>();


                var foundRoles = await _unitOfWork.Roles.FindAll();

                var roles = foundRoles.Adapt<IEnumerable<RoleViewModel>>();

                result.WithValue(roles);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
