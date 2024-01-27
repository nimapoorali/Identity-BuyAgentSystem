using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel.Rules;
public class FixedLengthRule : IBusinessRule
{
    public string? Value { get; }
    public string FieldName { get; }

    public int Length { get; }

    public string Message => string.Format(Validations.InvalidLengthField, FieldName, Length);

    public bool IsBroken()
    {
        return Value?.Length != Length;
    }

    public FixedLengthRule(string? value, int length, string fieldName)
    {
        Value = value;
        FieldName = fieldName;
        Length = length;
    }
}
