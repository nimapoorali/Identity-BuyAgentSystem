using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Roles.Queries.ViewModels;
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

namespace Identity.Application.Features.Roles.Queries
{
    public class GetRoleQuery : FluentResultRequest<RoleViewModel?>
    {
        public Guid Id { get; set; }

        public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, Result<RoleViewModel?>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetRoleQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<RoleViewModel?>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<RoleViewModel?>();


                var foundRole = await _unitOfWork.Roles.FindByIdAsync(request.Id);

                var role = foundRole?.Adapt<RoleViewModel>();

                result.WithValue(role);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
