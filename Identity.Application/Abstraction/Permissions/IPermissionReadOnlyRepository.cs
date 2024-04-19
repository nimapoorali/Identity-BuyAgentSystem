using Identity.Domain.Models.Aggregates.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction.Permissions
{
    public interface IPermissionReadOnlyRepository : IReadOnlyRepository<Permission>
    {
    }
}
