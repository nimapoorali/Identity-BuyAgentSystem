using Identity.Application.Abstraction.Roles;
using Identity.Application.Abstraction;
using Identity.Infrastructure.Persistence.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class IdentityReadOnlyUnitOfWork : IIdentityReadOnlyUnitOfWork
    {
        public IdentityDbContext DatabaseContext { get; }
        public IRoleReadOnlyRepository Roles { get; }

        public IdentityReadOnlyUnitOfWork(IdentityDbContext databaseContext)
        {
            DatabaseContext = databaseContext;

            Roles = new RoleReadOnlyRepository(DatabaseContext);
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
