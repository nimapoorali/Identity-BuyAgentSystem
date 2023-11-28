using FluentResults;
using Identity.Domain.Models.SeedWork;
using Identity.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel
{
    public class Mobile : ValueObject
    {
        public const int Length = 11;
        public const string RegularExpression = @"09\d{9}";
        public const int VerificationKeyLength = 5;

        public string Value { get; }
        public bool IsVerified { get; }
        public string? VerificationKey { get; }
        public DateTimeP? KeyExpirationDate { get; }

        private Mobile() : base()
        {
        }
        private Mobile(string value, bool isVerified, string? verificationKey, DateTimeP? keyExpirationDate) : this()
        {
            Value = value;
            IsVerified = isVerified;
            VerificationKey = verificationKey;
            KeyExpirationDate = keyExpirationDate;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string? ToString()
        {
            return Value;
        }

        public static Mobile Create(string value, bool isVerified = false, string? verificationKey = null, DateTimeP? keyExpirationDate = null)
        {
            //value = value.Fix();

            CheckRule(new FieldValueIsRequiredRule(value, IdentityDataDictionary.Mobile));

            CheckRule(new FixedLengthRule(value, Length, IdentityDataDictionary.Mobile));

            CheckRule(new RegexMatchedRule(value!, RegularExpression, IdentityDataDictionary.Mobile));

            if (keyExpirationDate is not null)
                CheckRule(new DateMustBeFutureRule(keyExpirationDate.Value, IdentityDataDictionary.MobileKeyExpirationDate));

            //var randomNumber = StringUtil.RandomNumeric(VerificationKeyLength);
            return new Mobile(value!, isVerified, verificationKey, keyExpirationDate);
        }

        public Mobile Verify(string? verificationKey = null)
        {
            CheckRule(new IsAlreadyInStateRule(IsVerified, true, IdentityDataDictionary.MobileIsVerified));

            //CheckRule(new ValuesMustBeEqualRule(IsVerified, true, IdentityDataDictionary.MobileIsVerified));

            if (VerificationKey is not null && verificationKey != VerificationKey)
            {
                throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.InvalidVerificationKey));
            }
            else
            {
                if (KeyExpirationDate is not null && KeyExpirationDate.Value < DateTime.Now)
                {
                    throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.ExpiredVerificationKey));
                }
                else
                {
                    return new Mobile(Value, true, VerificationKey, KeyExpirationDate);
                }
            }
        }
    }
}
