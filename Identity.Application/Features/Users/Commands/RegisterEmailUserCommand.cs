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
    public class RegisterEmailUserCommand : FluentResultRequest<NewUserViewModel>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public class RegisterUserByEmailCommandHandler : IRequestHandler<RegisterEmailUserCommand, Result<NewUserViewModel>>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public RegisterUserByEmailCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result<NewUserViewModel>> Handle(RegisterEmailUserCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewUserViewModel>();

                try
                {
                    var username = Domain.Models.Aggregates.Users.ValueObjects
                        .Username.Create(request.Username ?? request.Email!);

                    var randomString = Guid.NewGuid().ToString();
                    var tempPassword = randomString + randomString.ToUpper();
                    var password = Domain.Models.Aggregates.Users.ValueObjects
                        .Password.Create(tempPassword, PasswordStrength.VeryStrong)
                        .Hash();

                    var nickName = request.NickName is null ? null : Name.Create(request.NickName);

                    var firstName = request.FirstName is null ? null : Name.Create(request.FirstName);

                    var lastName = request.LastName is null ? null : Name.Create(request.LastName);

                    var verificationKey = Guid.NewGuid().ToString();
                    var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(10));
                    var email = request.Email is null ? null :
                        NP.Shared.Domain.Models.SharedKernel.Email.Create(request.Email, false, verificationKey, keyExpirationDate);

                    var activityState = Enumeration.FromValue<ActivityState>(ActivityState.Deactive.Value);

                    var user = User.Create(username, password, nickName, firstName, lastName, null, email, activityState);

                    UnitOfWork.Users.AddAsync(user);
                    await UnitOfWork.SaveChangesAsync();

                    var newUser = user.Adapt<NewUserViewModel>();

                    result.WithValue(newUser);
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
