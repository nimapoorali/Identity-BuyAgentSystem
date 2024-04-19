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
    public class GetPermissionQuery : FluentResultRequest<PermissionViewModel?>
    {
        public Guid Id { get; set; }

        public class GetPermissionQueryHandler : IRequestHandler<GetPermissionQuery, Result<PermissionViewModel?>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetPermissionQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<PermissionViewModel?>> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<PermissionViewModel?>();


                var foundPermission = await _unitOfWork.Permissions.FindByIdAsync(request.Id);

                var permission = foundPermission?.Adapt<PermissionViewModel>();

                result.WithValue(permission);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
