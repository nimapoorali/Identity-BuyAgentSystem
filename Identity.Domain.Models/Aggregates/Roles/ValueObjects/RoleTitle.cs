using FluentResults;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Roles.ValueObjects
{
    public class RoleTitle : ValueObject
    {
        public const int MinLength = 4;
        public const int MaxLength = 50;

        public string Title { get; }

        private RoleTitle() : base()
        {

        }
        private RoleTitle(string title) : this()
        {
            Title = title;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Title;
        }

        public static RoleTitle Create(string title)
        {
            title = title!.Fix();

            CheckRule(new FieldValueIsRequiredRule(title, IdentityDataDictionary.RoleTitle));

            CheckRule(new LengthMustBeInRangeRule(title, MinLength, MaxLength, IdentityDataDictionary.RoleTitle));

            return new RoleTitle(title);
        }


    }
}
