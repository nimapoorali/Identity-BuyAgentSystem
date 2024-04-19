using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Models
{
    public class Token
    {
        public string Value { get; init; } = "";
        public DateTime ExpiresAt { get; init; }
        public string Username { get; init; } = "";
        public string[]? Roles { get; init; }
    }
}
