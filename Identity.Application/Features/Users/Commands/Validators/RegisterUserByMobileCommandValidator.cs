using FluentValidation;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.Validators
{
    public class RegisterUserByMobileCommandValidator : AbstractValidator<RegisterMobileUserCommand>
    {
        public RegisterUserByMobileCommandValidator()
        {
            RuleFor(t => t.Mobile)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));
        }
    }
}
