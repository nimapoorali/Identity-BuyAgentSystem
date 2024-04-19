﻿using FluentResults;
using Identity.Application.Abstraction;
using Identity.Application.Features.Users.Commands.ViewModels;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
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
    public class VerifyMobileCommand : FluentResultRequest
    {
        public Guid? UserId { get; set; }
        public string? VerificationKey { get; set; }

        public class VerifyMobileCommandHandler : IRequestHandler<VerifyMobileCommand, Result>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public VerifyMobileCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                IdentityUnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(VerifyMobileCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    if (request.VerificationKey is null)
                        throw new BusinessRuleValidationException(
                            string.Format(Validations.RequiredField, SharedDataDictionary.MobileVerificationKey));

                    if (request.UserId is null)
                        throw new BusinessRuleValidationException(
                            string.Format(Validations.RequiredField, IdentityDataDictionary.UserId));


                    var mobileUser = await IdentityUnitOfWork.Users.FindByIdAsync(request.UserId.Value);

                    if (mobileUser is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!mobileUser.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (mobileUser.Mobile is null)
                        throw new BusinessRuleValidationException(IdentityValidations.NoMobileSet);

                    mobileUser.VerifyMobile(request.VerificationKey);


                    IdentityUnitOfWork.Users.Update(mobileUser);
                    await IdentityUnitOfWork.SaveChangesAsync();

                    var activateUser = mobileUser.Adapt<NewUserViewModel>();

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
