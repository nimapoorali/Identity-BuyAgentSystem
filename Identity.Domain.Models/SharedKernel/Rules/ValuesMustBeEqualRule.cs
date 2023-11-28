using Identity.Domain.Models.SeedWork;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel.Rules;
public class ValuesMustBeEqualRule : IBusinessRule
{
    public string FieldName { get; }

    public int? FirstValue { get; }
    public int? SecondValue { get; }

    public string Message => string.Format(Validations.ValuesNotEqualForField, FieldName);

    public bool IsBroken()
    {
        return
            FirstValue is null ||
            SecondValue is null ||
            FirstValue != SecondValue;
    }

    public ValuesMustBeEqualRule(object? firstValue, object? secondValue, string fieldName)
    {
        FieldName = fieldName;
        FirstValue = firstValue is null ? null : firstValue.GetHashCode();
        SecondValue = secondValue is null ? null : secondValue.GetHashCode();
    }

}
