using Identity.Application.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories.Roles
{
    public class RoleReadOnlyRepository : ReadOnlyRepository<Role>, IRoleReadOnlyRepository
    {
        public RoleReadOnlyRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

    }
}
