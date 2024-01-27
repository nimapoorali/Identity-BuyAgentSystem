using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel;
public class LengthMustBeSmallerThanRule : IBusinessRule
{
    public string? Value { get; }
    public string FieldName { get; }

    public int MaxLength { get; }

    public string Message => string.Format(Validations.LengthTooLongForField, FieldName, MaxLength);

    public bool IsBroken()
    {
        return Value?.Length > MaxLength;
    }

    public LengthMustBeSmallerThanRule(string? value, int maxLength, string fieldName)
    {
        Value = value;
        FieldName = fieldName;
        MaxLength = maxLength;
    }
}
