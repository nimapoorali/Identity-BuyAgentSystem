using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel;
public class RegexMatchedRule : IBusinessRule
{
    public string Value { get; }
    public string RegexFormat { get; }
    public string FieldName { get; }

    public string Message => string.Format(Validations.FormatNotMatched, FieldName);

    public bool IsBroken()
    {
        return !Regex.IsMatch(Value, RegexFormat);
    }

    public RegexMatchedRule(string value, string regexFormat, string fieldName)
    {
        Value = value;
        RegexFormat = regexFormat;
        FieldName = fieldName;
    }
}
