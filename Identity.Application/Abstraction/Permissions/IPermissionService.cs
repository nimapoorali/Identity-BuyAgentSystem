using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction.Permissions
{
    public interface IPermissionService
    {
        Task<bool> UserHasPermission(Guid userId, string permissionName);
        void UserPermissionsChanged(Guid userId);
        void PermissionChanged(string permissionName);
    }
}
