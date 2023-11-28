using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Permissions.Queries.ViewModels;
using Mapster;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Queries
{
    public class GetPermissionRolesQuery : FluentResultRequest<IEnumerable<PermissionRoleViewModel>>
    {
        public Guid Id { get; set; }

        public class GetPermissionRolesQueryHandler : IRequestHandler<GetPermissionRolesQuery, Result<IEnumerable<PermissionRoleViewModel>>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetPermissionRolesQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<IEnumerable<PermissionRoleViewModel>>> Handle(GetPermissionRolesQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<IEnumerable<PermissionRoleViewModel>>();


                var foundPermission = await _unitOfWork.Permissions.FindByIdAsync(request.Id);

                var roles = foundPermission?.Roles.Adapt<IEnumerable<PermissionRoleViewModel>>();

                if (roles == null)
                    roles = new List<PermissionRoleViewModel>();

                result.WithValue(roles);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
