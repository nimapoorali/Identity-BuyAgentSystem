using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.ViewModels
{
    public class NewUserViewModel
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? ActivityState { get; set; }
        //public string? MobileVerificationKey { get; set; }
        //public string? EmailVerificationKey { get; set; }
    }
}
