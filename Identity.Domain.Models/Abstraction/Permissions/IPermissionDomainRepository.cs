using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Abstraction.Permissions
{
    public interface IPermissionDomainRepository
    {
        Task<bool> IsNameExists(PermissionName name, CancellationToken cancellationToken = default);
        Task<bool> IsNameExists(PermissionName name, Guid currentPermissionId, CancellationToken cancellationToken = default);

    }
}
