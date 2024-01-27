using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SeedWork
{
    public interface IBusinessRuleAsync
    {
        Task<bool> IsBroken();

        string Message { get; }
    }
}
