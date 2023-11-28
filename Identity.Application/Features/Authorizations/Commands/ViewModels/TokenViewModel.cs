using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Authorizations.Commands.ViewModels
{
    public class TokenViewModel
    {
        public string? Token { get; set; }
        public string? ExpireTime { get; set; }
        public string? Username { get; set; }
        public string[]? Roles { get; set; }
    }
}
