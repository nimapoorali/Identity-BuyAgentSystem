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
    public class GetAllPermissionsQuery : FluentResultRequest<IEnumerable<PermissionViewModel>>
    {
        public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<IEnumerable<PermissionViewModel>>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetAllPermissionsQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<IEnumerable<PermissionViewModel>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<IEnumerable<PermissionViewModel>>();


                var foundPermissions = await _unitOfWork.Permissions.FindAll();

                var permissions = foundPermissions.Adapt<IEnumerable<PermissionViewModel>>();

                result.WithValue(permissions);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
