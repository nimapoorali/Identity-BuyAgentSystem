using Identity.Domain.Models.SeedWork;
using Identity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SharedKernel
{
    public class ActivityState : Enumeration
    {
        public const int NameMaxLength = 10;
     
        public static readonly ActivityState Active = new(1, IdentityDataDictionary.Active);
        public static readonly ActivityState Deactive = new(2, IdentityDataDictionary.Deactive);
        public static readonly ActivityState Suspend = new(3, IdentityDataDictionary.Suspend);

        private ActivityState() : base()
        {

        }
        private ActivityState(int value, string name) : base(value, name)
        {
        }
    }
}
