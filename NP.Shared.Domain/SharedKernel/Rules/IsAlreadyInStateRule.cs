using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel.Rules;
public class IsAlreadyInStateRule : IBusinessRule
{
    public string FieldName { get; }

    public int? CurrentState { get; }
    public int? NewState { get; }

    public string Message => string.Format(Validations.AlreadyInState, FieldName);

    public bool IsBroken()
    {
        return CurrentState == NewState;
    }

    public IsAlreadyInStateRule(int? currentState, int? newState, string fieldName)
    {
        FieldName = fieldName;
        CurrentState = currentState;
        NewState = newState;
    }

    public IsAlreadyInStateRule(bool currentState, bool newState, string fieldName)
    {
        FieldName = fieldName;
        CurrentState = currentState ? 1 : 0;
        NewState = newState ? 1 : 0;
    }
}
