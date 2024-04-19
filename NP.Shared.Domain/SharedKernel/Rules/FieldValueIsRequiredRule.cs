using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel.Rules;
public class FieldValueIsRequiredRule : IBusinessRule
{
    public string? Value { get; }
    public string FieldName { get; }

    public string Message => string.Format(Validations.RequiredField, FieldName);

    public bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(Value);
    }

    public FieldValueIsRequiredRule(string? value, string fieldName)
    {
        Value = value;
        FieldName = fieldName;
    }

    public FieldValueIsRequiredRule(Guid? value, string fieldName)
    {
        Value = (value is null || value.Value == Guid.Empty) ? null : value!.ToString();
        FieldName = fieldName;
    }
}
