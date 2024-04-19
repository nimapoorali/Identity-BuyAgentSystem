using Identity.Application.Abstraction.Permissions;
using Identity.Domain.Models.Abstraction.Permissions;
using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories.Permissions
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository, IPermissionDomainRepository
    {
        public PermissionRepository(IdentityDbContext dbContext) : base(dbContext)
        {

        }

        public Task<bool> IsNameExists(PermissionName name, CancellationToken cancellationToken = default)
        {
            return Context.Permissions!.AnyAsync(r => r.Name == name);
        }

        public Task<bool> IsNameExists(PermissionName name, Guid currentPermissionId, CancellationToken cancellationToken = default)
        {
            return Context.Permissions!.AnyAsync(r => r.Name == name && r.Id != currentPermissionId);
        }
    }
}
