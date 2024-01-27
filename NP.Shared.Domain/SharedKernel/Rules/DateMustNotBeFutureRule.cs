using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel.Rules;
public class DateMustNotBeFutureRule : IBusinessRule
{
    public DateTimeP Date { get; }
    public string FieldName { get; }

    public string Message => string.Format(Validations.InvalidCurrentDateField, FieldName);

    public bool IsBroken()
    {
        return Date.Value > DateTime.Now;
    }

    public DateMustNotBeFutureRule(DateTimeP value, string fieldName)
    {
        Date = value;
        FieldName = fieldName;
    }
}
