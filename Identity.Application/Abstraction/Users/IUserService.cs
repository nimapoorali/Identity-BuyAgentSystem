using Identity.Domain.Models.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction.Users
{
    public interface IUserService
    {
        Task<User?> GetUser(string username, string password);
        Task<User?> GetMobileUser(string mobile, string key);
        Task<User?> GetEmailUser(string email, string key);

        Task NewMobileUserVerificationKey(string mobile);
        Task NewEmailUserVerificationKey(string email);
    }
}
