using FluentValidation;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Roles.Commands.Validators
{
    public class UpdateRoleStatusCommandValidator : AbstractValidator<UpdateRoleStatusCommand>
    {
        public UpdateRoleStatusCommandValidator()
        {
            RuleFor(t => t.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));


            RuleFor(t => t.ActivityStatus)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(string.Format(Validations.RequiredField, "{PropertyName}"));


        }
    }
}
