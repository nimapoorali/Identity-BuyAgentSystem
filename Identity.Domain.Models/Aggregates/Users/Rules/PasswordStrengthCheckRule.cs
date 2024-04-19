using NP.Shared.Domain.Models.SeedWork;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Users.Rules
{
    public class PasswordStrengthCheckRule : IBusinessRule
    {
        public string Value { get; }
        public PasswordStrength MinimumPasswordStrength { get; }

        public string Message => IdentityValidations.InsufficientPasswordStrength;

        public bool IsBroken()
        {
            var strength = PasswordCheck.GetPasswordStrength(Value);
            return strength < MinimumPasswordStrength;
        }

        public PasswordStrengthCheckRule(string value, PasswordStrength minimumStrength)
        {
            Value = value;
            MinimumPasswordStrength = minimumStrength;
        }
    }
}
