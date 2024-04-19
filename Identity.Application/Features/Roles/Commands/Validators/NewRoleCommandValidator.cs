using FluentValidation;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Roles.Commands.Validators
{
    public class NewRoleCommandValidator : AbstractValidator<NewRoleCommand>
    {
        public NewRoleCommandValidator()
        {
            RuleFor(t => t.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));

            RuleFor(t => t.GroupTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));

        }
    }
}
