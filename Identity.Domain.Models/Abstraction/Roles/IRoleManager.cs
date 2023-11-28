using FluentResults;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Abstraction.Roles
{
    public interface IRoleManager
    {
        Task<Result<bool>> IsTitleExists(RoleTitle roleTitle);
    }
}
