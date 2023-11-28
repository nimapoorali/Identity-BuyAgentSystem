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
    public class GetTokenCommandValidator : AbstractValidator<TokenCommand>
    {
        public GetTokenCommandValidator()
        {
            RuleFor(t => t.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));

            RuleFor(t => t.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));
        }
    }
}
