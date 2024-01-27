using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel.Rules;
public class DateMustBeFutureRule : IBusinessRule
{
    public DateTime Value { get; }
    public string FieldName { get; }

    public string Message => string.Format(Validations.InvalidFutureDateField, FieldName);

    public bool IsBroken()
    {
        return Value <= DateTime.Now;
    }

    public DateMustBeFutureRule(DateTime value, string fieldName)
    {
        Value = value;
        FieldName = fieldName;
    }
}
