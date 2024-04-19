using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Mapster;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands
{
    public class UpdateUserCommand : FluentResultRequest<NewUserViewModel>
    {
        public Guid? Id { get; set; }
        public string? NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ActivityState { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<NewUserViewModel>>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdateUserCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result<NewUserViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewUserViewModel>();

                try
                {
                    var nickName = request.NickName is null ? null : Name.Create(request.NickName);
                    var firstName = request.FirstName is null ? null : Name.Create(request.FirstName);
                    var lastName = request.LastName is null ? null : Name.Create(request.LastName);

                    var user = await UnitOfWork.Users.FindByIdAsync(request.Id!.Value);

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    var activityState = request.ActivityState is null ? NP.Shared.Domain.Models.SharedKernel.ActivityState.Deactive :
                          Enumeration.FromName<ActivityState>(request.ActivityState);

                    user.Chagne(nickName, firstName,lastName, activityState);

                    UnitOfWork.Users.Update(user);
                    await UnitOfWork.SaveChangesAsync();

                    var userVieModel = user.Adapt<NewUserViewModel>();

                    result.WithValue(userVieModel);
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
