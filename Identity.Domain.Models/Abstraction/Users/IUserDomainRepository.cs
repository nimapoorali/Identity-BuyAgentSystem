using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Abstraction.Users
{
    public interface IUserDomainRepository
    {
        Task<bool> IsUsernameExists(Username username, CancellationToken cancellationToken = default);
        Task<bool> IsUsernameExists(Username username, Guid currentUserId, CancellationToken cancellationToken = default);

    }
}
