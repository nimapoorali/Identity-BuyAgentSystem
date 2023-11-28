using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Permissions;
using Identity.Application.Abstraction.Roles;
using Identity.Application.Abstraction.Users;
using Identity.Infrastructure.Persistence.Repositories.Permissions;
using Identity.Infrastructure.Persistence.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        public IdentityDbContext DatabaseContext { get; }
        public IRoleRepository Roles { get; }
        public IUserRepository Users { get; }
        public IPermissionRepository Permissions { get; }

        public IdentityUnitOfWork(IdentityDbContext databaseContext)
        {
            DatabaseContext = databaseContext;

            Roles = new RoleRepository(DatabaseContext);
            Users = new UserRepository(DatabaseContext);
            Permissions = new PermissionRepository(DatabaseContext);
        }


        public bool IsDisposed => throw new NotImplementedException();

        public void Dispose()
        {
            DatabaseContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task<int> SaveChangesAsync()
        {
            return DatabaseContext.SaveChangesAsync();
        }
    }
}
