using Identity.Application.Abstraction.Permissions;
using Identity.Domain.Models.Aggregates.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories.Permissions
{
    public class PermissionReadOnlyRepository : ReadOnlyRepository<Permission>, IPermissionReadOnlyRepository
    {
        public PermissionReadOnlyRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

    }
}
