using Identity.Domain.Models.SeedWork;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel.Rules;
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
