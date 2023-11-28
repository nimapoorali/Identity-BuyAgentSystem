using FluentResults;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Abstraction.Users
{
    public interface IUserManager
    {
        Task<Result<bool>> IsUsernameExists(Username username);
        Task<Result<bool>> IsUsernameExistsInOthers(Username username, User currentUser);
    }
}
