using FluentResults;
using Identity.Domain.Models.SeedWork;
using Identity.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel
{
    public class Email : ValueObject
    {
        public const string RegularExpression = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
        public const int VerificationKeyLength = 20;

        public string Value { get; }
        public bool IsVerified { get; }
        public string? VerificationKey { get; }
        public DateTimeP? KeyExpirationDate { get; }

        private Email() : base()
        {
        }
        private Email(string value) : this()
        {
            Value = value;
        }
        private Email(string value, bool isVerified, string? verificationKey, DateTimeP? keyExpirationDate) : this()
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

        public static Email Create(string value, bool isVerified = false, string? verificationKey = null, DateTimeP? keyExpirationDate = null)
        {
            //value = value.Fix();

            CheckRule(new FieldValueIsRequiredRule(value, IdentityDataDictionary.Email));

            CheckRule(new RegexMatchedRule(value!, RegularExpression, IdentityDataDictionary.Email));

            try
            {
                MailAddress m = new(value!);
            }
            catch (FormatException)
            {
                throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.InvalidEmail));
            }

            if (keyExpirationDate is not null)
                CheckRule(new DateMustBeFutureRule(keyExpirationDate.Value, IdentityDataDictionary.MobileKeyExpirationDate));

            //var randomAlphaNumeric = StringUtil.RandomAlphaNumeric(VerificationKeyLength);
            return new Email(value, isVerified, verificationKey, keyExpirationDate);
        }

        public Email Verify(string? verificationKey = null)
        {

            CheckRule(new IsAlreadyInStateRule(IsVerified, true, IdentityDataDictionary.EmailIsVerified));

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
                    return new Email(Value, true, VerificationKey, KeyExpirationDate);
                }
            }
        }
    }
}
