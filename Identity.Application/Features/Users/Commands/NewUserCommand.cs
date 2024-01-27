using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
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
    public class NewUserCommand : FluentResultRequest<NewUserViewModel>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? ActivityState { get; set; }

        public class NewUserCommandHandler : IRequestHandler<NewUserCommand, Result<NewUserViewModel>>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public NewUserCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result<NewUserViewModel>> Handle(NewUserCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewUserViewModel>();

                try
                {
                    var username = Domain.Models.Aggregates.Users.ValueObjects
                        .Username.Create(request.Username!);

                    var password = Domain.Models.Aggregates.Users.ValueObjects
                        .Password.Create(request.Password!, PasswordStrength.VeryStrong)
                        .Hash();

                    var nickName = request.NickName is null ? null : Name.Create(request.NickName);

                    var firstName = request.FirstName is null ? null : Name.Create(request.FirstName);

                    var lastName = request.LastName is null ? null : Name.Create(request.LastName);

                    var mobile = request.Mobile is null ? null :
                        NP.Shared.Domain.Models.SharedKernel.Mobile.Create(request.Mobile, true, null, null);

                    var email = request.Email is null ? null :
                        NP.Shared.Domain.Models.SharedKernel.Email.Create(request.Email, true, null, null);

                    var activityState = request.ActivityState is null ? NP.Shared.Domain.Models.SharedKernel.ActivityState.Deactive :
                          Enumeration.FromName<ActivityState>(request.ActivityState);


                    var user = User.Create(username, password, nickName, firstName, lastName, mobile, email, activityState);

                    UnitOfWork.Users.AddAsync(user);
                    await UnitOfWork.SaveChangesAsync();

                    var userViewModel = user.Adapt<NewUserViewModel>();

                    result.WithValue(userViewModel);
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
