using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using Identity.Domain.Models.Aggregates.Users.Rules;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using Mapster;
using MediatR;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NP.Shared.Domain.Models.SharedKernel;

namespace Identity.Application.Features.Users.Commands
{
    public class UpdateMobileCommand : FluentResultRequest
    {
        public Guid? UserId { get; set; }
        public string? Mobile { get; set; }

        public class UpdateMobileCommandHandler : IRequestHandler<UpdateMobileCommand, Result>
        {
            private readonly IIdentityUnitOfWork UnitOfWork;

            public UpdateMobileCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(UpdateMobileCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    var user = await UnitOfWork.Users.FindByIdAsync(request.UserId!.Value);

                    if (user is null)
                        throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

                    var verificationKey = StringUtil.RandomNumeric(5);
                    var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(2));
                    var mobile = request.Mobile is null ? null :
                        NP.Shared.Domain.Models.SharedKernel.Mobile.Create(request.Mobile, false, verificationKey, keyExpirationDate);

                    if (mobile is null)
                        throw new BusinessRuleValidationException(IdentityValidations.MobileCantSet);

                    user.ChangeMobile(mobile);

                    UnitOfWork.Users.Update(user);
                    await UnitOfWork.SaveChangesAsync();

                    result.WithSuccess(Messages.OperationSucceeded);

                    return result;
                }
                catch (BusinessRuleValidationException ex)
                {
                    result.WithError(ex.Message);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
