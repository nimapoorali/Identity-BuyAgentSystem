using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.ViewModels
{
    public class UserRolesViewModel
    {
        public Guid? UserId { get; set; }
        public Guid[]? RoleIds { get; set; }
    }
}
