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
    public class RoleGroupTitle : ValueObject
    {
        public const int MinLength = 5;
        public const int MaxLength = 50;

        public string Title { get; }

        private RoleGroupTitle() : base()
        {

        }
        private RoleGroupTitle(string title) : this()
        {
            Title = title;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Title;
        }

        public static RoleGroupTitle Create(string title)
        {
            CheckRule(new FieldValueIsRequiredRule(title, IdentityDataDictionary.RoleGroupTitle));

            title = title.Fix();

            CheckRule(new LengthMustBeInRangeRule(title, MinLength, MaxLength, IdentityDataDictionary.RoleGroupTitle));

            return new RoleGroupTitle(title);
        }
    }
}
