using NP.Shared.Domain.Models.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NP.Resources;

namespace NP.Shared.Domain.Models.SharedKernel
{
    public class ActivityState : Enumeration
    {
        public const int NameMaxLength = 10;

        public static readonly ActivityState Active = new(1, SharedDataDictionary.Active);
        public static readonly ActivityState Deactive = new(2, SharedDataDictionary.Deactive);
        public static readonly ActivityState Suspend = new(3, SharedDataDictionary.Suspend);

        private ActivityState() : base()
        {

        }
        private ActivityState(int value, string name) : base(value, name)
        {
        }
    }
}
