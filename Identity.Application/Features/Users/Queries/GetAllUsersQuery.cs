using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Queries.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
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
    public class GetAllUsersQuery : FluentResultRequest<IEnumerable<UserViewModel>>
    {
        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserViewModel>>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public GetAllUsersQueryHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<IEnumerable<UserViewModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var result = new Result<IEnumerable<UserViewModel>>();


                var foundUsers = await _unitOfWork.Users.FindAll();

                var users = foundUsers.Adapt<IEnumerable<UserViewModel>>();

                result.WithValue(users);
                result.WithSuccess(Messages.OperationSucceeded);

                return result;
            }
        }
    }
}
