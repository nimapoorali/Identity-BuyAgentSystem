using FluentValidation;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Commands.Validators
{
    public class NewPermissionCommandValidator : AbstractValidator<NewPermissionCommand>
    {
        public NewPermissionCommandValidator()
        {
            RuleFor(t => t.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));


        }
    }
}
