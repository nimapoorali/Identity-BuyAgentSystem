using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Commands.ViewModels
{
    public class PermissionRolesViewModel
    {
        public Guid? PermissionId { get; set; }
        public Guid[]? RoleIds { get; set; }
    }
}
