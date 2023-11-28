using Identity.Domain.Models.Aggregates.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction.Roles
{
    public interface IRoleReadOnlyRepository : IReadOnlyRepository<Role>
    {
    }
}
