using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using Identity.Domain.Models.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction.Users
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<bool> UserHasPermission(Guid userId, PermissionName name, CancellationToken cancellationToken = default);

    }
}
