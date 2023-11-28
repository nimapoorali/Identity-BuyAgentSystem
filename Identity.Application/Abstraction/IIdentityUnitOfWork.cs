using Identity.Application.Abstraction.Permissions;
using Identity.Application.Abstraction.Roles;
using Identity.Application.Abstraction.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction
{
    public interface IIdentityUnitOfWork : IUnitOfWork
    {
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }
        IPermissionRepository Permissions { get; }
    }
}
