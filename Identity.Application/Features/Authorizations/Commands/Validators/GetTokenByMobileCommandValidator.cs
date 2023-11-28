using FluentValidation;
using Identity.Application.Features.Authorizations.Commands;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.Validators
{
    public class GetTokenByMobileCommandValidator : AbstractValidator<MobileTokenCommand>
    {
        public GetTokenByMobileCommandValidator()
        {
            RuleFor(t => t.Mobile)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));

            RuleFor(t => t.VerificationKey)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));
        }
    }
}
