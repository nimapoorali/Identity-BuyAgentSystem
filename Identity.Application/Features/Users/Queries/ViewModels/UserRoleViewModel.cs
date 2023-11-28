using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Queries.ViewModels
{
    public class UserRoleViewModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
