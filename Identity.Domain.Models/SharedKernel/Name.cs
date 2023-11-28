using FluentResults;
using Identity.Domain.Models.SeedWork;
using Identity.Domain.Models.SharedKernel;
using Identity.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel
{
    public class Name : ValueObject
    {
        public const int MinLength = 4;
        public const int MaxLength = 100;

        public string Value { get; }

        private Name() : base()
        {

        }
        private Name(string value) : this()
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Name Create(string value)
        {
            value = value.Fix();

            CheckRule(new FieldValueIsRequiredRule(value, IdentityDataDictionary.Name));
            CheckRule(new LengthMustBeInRangeRule(value, MinLength, MaxLength, IdentityDataDictionary.Name));

            return new Name(value);
        }
    }
}
