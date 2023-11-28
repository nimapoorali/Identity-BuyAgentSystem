using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Abstraction.Roles
{
    public interface IRoleDomainRepository
    {
        Task<bool> IsTitleExists(RoleTitle roleTitle, CancellationToken cancellationToken = default);
        Task<bool> IsTitleExists(RoleTitle roleTitle, Guid currentRoleId, CancellationToken cancellationToken = default);

    }
}
