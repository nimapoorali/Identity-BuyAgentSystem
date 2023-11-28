using Identity.Domain.Models.SeedWork;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel;
public class LengthMustBeInRangeRule : IBusinessRule
{
    public string? Value { get; }
    public string FieldName { get; }

    public int MinLength { get; }
    public int MaxLength { get; }

    public string Message => string.Format(Validations.LengthNotInRangeField, FieldName);

    public bool IsBroken()
    {
        return !(Value?.Length >= MinLength && Value?.Length <= MaxLength);
    }

    public LengthMustBeInRangeRule(string? value, int minLength, int maxLength, string fieldName)
    {
        Value = value;
        FieldName = fieldName;
        MinLength = minLength;
        MaxLength = maxLength;
    }
}
