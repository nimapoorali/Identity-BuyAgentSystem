﻿using FluentValidation;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.Validators
{
    public class ActivateUserByEmailCommandValidator : AbstractValidator<ActivateEmailUserCommand>
    {
        public ActivateUserByEmailCommandValidator()
        {
            //RuleFor(t => t.Username)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));

            RuleFor(t => t.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));
        }
    }
}
