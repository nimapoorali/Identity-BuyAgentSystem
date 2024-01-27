using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using Identity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services.Aggregates.Users.Rules
{
    public class UsernameMustNotExistRule : IBusinessRuleAsync
    {
        public IUserManager UserManager { get; }
        public Username Username { get; }

        public string Message => IdentityValidations.DuplicateUsername;

        public async Task<bool> IsBroken()
        {
            var result = await UserManager.IsUsernameExists(Username);

            return result.IsSuccess && result.Value;
        }

        public UsernameMustNotExistRule(Username title, IUserManager userManager)
        {
            Username = title;
            UserManager = userManager;
        }
    }
}
