using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application
{
    public class Configs
    {
        public string JWTSecret { get; set; }
        public int JWTTimeout { get; set; }
    }
}
