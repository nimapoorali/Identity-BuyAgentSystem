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
using Identity.Domain.Models.Aggregates.Users.Rules;

namespace Identity.Domain.Models.Aggregates.Users.ValueObjects
{
    public class Password : ValueObject
    {
        public string Value { get; }
        public bool IsHashed { get; private set; }

        private Password() : base()
        {
        }

        private Password(string value, bool isHashed = false) : this()
        {
            Value = value;
            IsHashed = isHashed;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value!;
        }

        public static Password Create(string value, PasswordStrength minimumPasswordStrength)
        {
            //value = value.Fix();

            CheckRule(new FieldValueIsRequiredRule(value, IdentityDataDictionary.Password));
            CheckRule(new PasswordStrengthCheckRule(value!, minimumPasswordStrength));

            return new Password(value!);
        }

        public Password Hash()
        {
            var hashedValue = PasswordHash.HashPassword(Value);
            return new Password(hashedValue, true);
        }

        public bool Validate(string password)
        {
            //if (!IsHashed)
            //    throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.PasswordNotHashed));

            return PasswordHash.ValidatePassword(password, Value);
        }
    }
}
