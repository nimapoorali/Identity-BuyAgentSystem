using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Users;
using Identity.Application.Features.Authorizations.Commands.ViewModels;
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

namespace Identity.Application.Features.Authorizations.Commands
{
    public class TokenCommand : FluentResultRequest<TokenViewModel>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public class TokenCommandHandler : IRequestHandler<TokenCommand, Result<TokenViewModel>>
        {
            private readonly ITokenService TokenService;
            public IUserService UserService { get; }
            public IIdentityUnitOfWork IdentityUnitOfWork { get; }

            public TokenCommandHandler(IUserService userService, ITokenService tokenService, IIdentityUnitOfWork identityUnitOfWork)
            {
                UserService = userService;
                TokenService = tokenService;
                IdentityUnitOfWork = identityUnitOfWork;
            }

            public async Task<Result<TokenViewModel>> Handle(TokenCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<TokenViewModel>();

                try
                {
                    //var username = Domain.Models.Aggregates.Users.ValueObjects
                    //    .Username.Create(request.Username!);

                    //var user = await UnitOfWork.Users.SingleAsync(user => user.Username == username);

                    //if (user is null)
                    //    throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    //if (!user.IsActive)
                    //    throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.NotActiveUser));

                    //if (!user.Password.Validate(request.Password!))
                    //    throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.IncorrectUsernameOrPasseword));

                    var user = await UserService.GetUser(request.Username!, request.Password!);
                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    var generatedToken = await TokenService.GenerateToken(user.Id);

                    var token = generatedToken.Adapt<TokenViewModel>();

                    //Added for client info
                    token.Username = user.Username.Value;
                    var roleIds = user.Roles.Select(r => r.RoleId).ToArray();
                    var roleNames = await IdentityUnitOfWork.Roles.FindAsync(r => roleIds.Contains(r.Id));
                    if (roleNames is not null)
                        token.Roles = roleNames.Select(r => r.Title.Title).ToArray();

                    result.WithValue(token);
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
