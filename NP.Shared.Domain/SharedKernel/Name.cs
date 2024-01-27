using FluentResults;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using NP.Resources;
using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel
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

            CheckRule(new FieldValueIsRequiredRule(value, SharedDataDictionary.Name));
            CheckRule(new LengthMustBeInRangeRule(value, MinLength, MaxLength, SharedDataDictionary.Name));

            return new Name(value);
        }
    }
}
