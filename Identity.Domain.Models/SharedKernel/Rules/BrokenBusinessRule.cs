using Identity.Domain.Models.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel.Rules
{
    public class BrokenBusinessRule : IBusinessRule
    {
        private string _message;
        public string Message => _message;

        public BrokenBusinessRule(string message)
        {
            _message = message;
        }

        public bool IsBroken()
        {
            return true;
        }
    }
}
