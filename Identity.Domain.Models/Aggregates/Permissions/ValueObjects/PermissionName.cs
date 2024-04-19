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

namespace Identity.Domain.Models.Aggregates.Permissions.ValueObjects
{
    public class PermissionName : ValueObject
    {
        public const int MinLength = 4;
        public const int MaxLength = 255;

        public string Name { get; }

        private PermissionName() : base()
        {

        }
        private PermissionName(string name) : this()
        {
            Name = name;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }

        public static PermissionName Create(string name)
        {
            name = name!.Fix();

            CheckRule(new FieldValueIsRequiredRule(name, IdentityDataDictionary.PermissionName));

            CheckRule(new LengthMustBeInRangeRule(name, MinLength, MaxLength, IdentityDataDictionary.PermissionName));

            return new PermissionName(name);
        }
    }
}
