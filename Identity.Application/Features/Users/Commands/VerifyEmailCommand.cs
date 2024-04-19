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
    public class VerifyEmailCommand : FluentResultRequest
    {
        public Guid? UserId { get; set; }
        public string? VerificationKey { get; set; }

        public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
        {
            private readonly IIdentityUnitOfWork IdentityUnitOfWork;

            public VerifyEmailCommandHandler(IIdentityUnitOfWork unitOfWork)
            {
                IdentityUnitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
            {
                var result = new Result();

                try
                {
                    if (request.VerificationKey is null)
                        throw new BusinessRuleValidationException(
                            string.Format(Validations.RequiredField, SharedDataDictionary.EmailVerificationKey));

                    if (request.UserId is null)
                        throw new BusinessRuleValidationException(
                            string.Format(Validations.RequiredField, IdentityDataDictionary.UserId));


                    var emailUser = await IdentityUnitOfWork.Users.FindByIdAsync(request.UserId.Value);

                    if (emailUser is null)
                        throw new BusinessRuleValidationException(Validations.NotExistsRecord);

                    if (!emailUser.IsActive)
                        throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

                    if (emailUser.Email is null)
                        throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

                    emailUser.VerifyEmail(request.VerificationKey);


                    IdentityUnitOfWork.Users.Update(emailUser);
                    await IdentityUnitOfWork.SaveChangesAsync();

                    var activateUser = emailUser.Adapt<NewUserViewModel>();

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
