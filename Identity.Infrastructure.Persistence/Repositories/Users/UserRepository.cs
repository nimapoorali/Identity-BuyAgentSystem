using Identity.Application.Abstraction.Users;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories.Roles
{
    public class UserRepository : Repository<User>, IUserRepository, IUserDomainRepository
    {
        public UserRepository(IdentityDbContext dbContext) : base(dbContext)
        {

        }

        public Task<bool> IsUsernameExists(Username username, CancellationToken cancellationToken = default)
        {
            return Context.Users!.AnyAsync(u => u.Username == username);
        }

        public Task<bool> IsUsernameExists(Username username, Guid currentUserId, CancellationToken cancellationToken = default)
        {
            return Context.Users!.AnyAsync(u => u.Username == username && u.Id != currentUserId);
        }

        //public Task<bool> UserHasPermission(Guid userId, PermissionName name, CancellationToken cancellationToken = default)
        //{

        //    return Context.Users.FirstAsync(u => u.Id == userId).Result.Roles.Any(ur => Context.Permissions.Where(r => r.Name == name).SelectMany(p => p.Roles).Select(r => r.RoleId).Contains(ur.RoleId));
        //}
    }
}
