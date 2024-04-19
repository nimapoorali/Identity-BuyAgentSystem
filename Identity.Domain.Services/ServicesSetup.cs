using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Domain.Services.Aggregates.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services
{
    public static class ServicesSetup
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IRoleManager, RoleManager>()
                .AddTransient<IUserManager, UserManager>();

        }
    }
}
