using FluentResults;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;

namespace Identity.Domain.Models.Aggregates.Users.ValueObjects
{
    public class Username : ValueObject
    {
        public const int MinLength = 5;
        public const int MaxLength = 20;

        //Only characters and digits are allowed and digits are not allowed at the first
        //public const string RegularExpression = "^[a-zA-Z][a-zA-Z0-9]*$";
        public const string RegularExpression = "[a-zA-Z0-9]*$";

        public string? Value { get; }

        private Username() : base()
        {
        }

        private Username(string value) : this()
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value!;
        }

        public override string ToString()
        {
            return Value!;
        }

        public static Username Create(string value)
        {
            //value = value.Fix();

            CheckRule(new FieldValueIsRequiredRule(value, IdentityDataDictionary.Username));

            CheckRule(new LengthMustBeInRangeRule(value, MinLength, MaxLength, IdentityDataDictionary.Username));

            CheckRule(new RegexMatchedRule(value!, RegularExpression, IdentityDataDictionary.Username));

            return new Username(value);
        }

       
    }
}
