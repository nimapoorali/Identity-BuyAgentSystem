using FluentResults;
using Identity.Domain.Models.Abstraction;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using Identity.Domain.Services.Aggregates.Users.Rules;
using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services.Aggregates.Users
{
    public class UserManager : DomainService, IUserManager
    {
        public IUserDomainRepository DomainRepo { get; }

        public UserManager(IUserDomainRepository domainRepo)
        {
            DomainRepo = domainRepo;
        }

        public Task<Result<bool>> IsUsernameExists(Username username)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> IsUsernameExistsInOthers(Username username, User currentUser)
        {
            throw new NotImplementedException();
        }
    }
}
