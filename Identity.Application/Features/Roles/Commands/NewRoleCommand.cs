using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
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
    public class NewRoleCommand : FluentResultRequest<NewRoleViewModel>
    {
        public string? Title { get; set; }
        public string? GroupTitle { get; set; }
        public string? ActivityState { get; set; }

        public class NewRoleCommandHandler : IRequestHandler<NewRoleCommand, Result<NewRoleViewModel>>
        {
            private readonly IIdentityUnitOfWork _unitOfWork;

            public NewRoleCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<NewRoleViewModel>> Handle(NewRoleCommand request, CancellationToken cancellationToken)
            {
                var result = new Result<NewRoleViewModel>();

                try
                {
                    var title = RoleTitle.Create(request.Title!);

                    var groupTitle = RoleGroupTitle.Create(request.GroupTitle!);

                    var activityState = request.ActivityState is null ? NP.Shared.Domain.Models.SharedKernel.ActivityState.Deactive :
                        Enumeration.FromName<ActivityState>(request.ActivityState);

                    var role = Role.Create(title, groupTitle, activityState);

                    _unitOfWork.Roles.AddAsync(role);
                    await _unitOfWork.SaveChangesAsync();


                    var newRole = role.Adapt<NewRoleViewModel>();

                    result.WithValue(newRole);
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
