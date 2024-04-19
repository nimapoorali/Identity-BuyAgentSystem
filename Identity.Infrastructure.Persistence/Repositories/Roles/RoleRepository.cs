using Identity.Application.Abstraction.Roles;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories.Roles
{
    public class RoleRepository : Repository<Role>, IRoleRepository, IRoleDomainRepository
    {
        public RoleRepository(IdentityDbContext dbContext) : base(dbContext)
        {

        }

        public Task<bool> IsTitleExists(RoleTitle roleTitle, CancellationToken cancellationToken = default)
        {
            return Context.Roles!.AnyAsync(r => r.Title == roleTitle);
        }

        public Task<bool> IsTitleExists(RoleTitle roleTitle, Guid currentRoleId, CancellationToken cancellationToken = default)
        {
            return Context.Roles!.AnyAsync(r => r.Title == roleTitle && r.Id != currentRoleId);
        }
    }
}
