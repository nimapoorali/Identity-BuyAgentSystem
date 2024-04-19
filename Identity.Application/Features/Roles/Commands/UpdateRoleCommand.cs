using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
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

namespace Identity.Application.Features.Roles.Commands
{
    public class UpdateRoleCommand : FluentResultRequest<NewRoleViewModel>
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? GroupTitle { get; set; }
        public string? ActivityState { get; set; }

        public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<NewRoleViewModel>>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdateRoleCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result<NewRoleViewModel>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewRoleViewModel>();

                try
                {
                    var title = RoleTitle.Create(request.Title!);

                    var groupTitle = RoleGroupTitle.Create(request.GroupTitle!);

                    //var activityState = Enumeration.FromValue<ActivityState>(
                    //    request.IsActive is null ? ActivityState.Deactive.Value :
                    //    request.IsActive.Value ? ActivityState.Active.Value :
                    //    ActivityState.Deactive.Value);

                    var role = await UnitOfWork.Roles.FindByIdAsync(request.Id!.Value);

                    if (role is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    var activityState = request.ActivityState is null ? NP.Shared.Domain.Models.SharedKernel.ActivityState.Deactive :
                          Enumeration.FromName<ActivityState>(request.ActivityState);

                    role.Change(title, groupTitle, activityState);

                    UnitOfWork.Roles.Update(role);
                    await UnitOfWork.SaveChangesAsync();

                    var roleVieModel = role.Adapt<NewRoleViewModel>();

                    result.WithValue(roleVieModel);
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
