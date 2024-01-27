using FluentResults;
using NP.Shared.Domain.Models.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services
{
    public abstract class DomainService
    {
        protected static void CheckRule<TValue>(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
        protected static void CheckRule<TValue>(IBusinessRule rule, Result<TValue> toResult)
        {
            if (rule.IsBroken())
            {
                toResult.WithError(rule.Message);
            }
        }
        protected static async Task CheckRule<TValue>(IBusinessRuleAsync rule, Result<TValue> toResult)
        {
            if (await rule.IsBroken())
            {
                toResult.WithError(rule.Message);
            }
        }

        protected static async Task CheckRule(IBusinessRuleAsync rule, Result toResult)
        {
            if (await rule.IsBroken())
            {
                toResult.WithError(rule.Message);
            }
        }
    }
}
