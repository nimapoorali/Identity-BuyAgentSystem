using FluentResults;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using Identity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services
{
    public class RoleManager : DomainService, IRoleManager
    {
        public IRoleDomainRepository DomainRepo { get; }

        public RoleManager(IRoleDomainRepository domainRepo)
        {
            DomainRepo = domainRepo;
        }

        public Task<Result<bool>> IsTitleExists(RoleTitle roleTitle)
        {
            throw new NotImplementedException();
        }
    }
}
