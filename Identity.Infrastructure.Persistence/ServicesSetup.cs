using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Permissions;
using Identity.Application.Abstraction.Roles;
using Identity.Application.Abstraction.Users;
using Identity.Domain.Models.Abstraction.Permissions;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Abstraction.Users;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Persistence.Repositories.Permissions;
using Identity.Infrastructure.Persistence.Repositories.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence
{
    public static class ServicesSetup
    {
        public static IServiceCollection AddIdentityPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDatabaseContext(configuration, "IdentityConnection")
                .AddScoped<IIdentityDbContext>(provider => provider.GetService<IdentityDbContext>()!)
                .AddTransient<IIdentityUnitOfWork, IdentityUnitOfWork>()

                .AddTransient(typeof(IRoleRepository), typeof(RoleRepository))
                .AddTransient(typeof(IRoleDomainRepository), typeof(RoleRepository))

                .AddTransient(typeof(IUserRepository), typeof(UserRepository))
                .AddTransient(typeof(IUserDomainRepository), typeof(UserRepository))

                .AddTransient(typeof(IPermissionRepository), typeof(PermissionRepository))
                .AddTransient(typeof(IPermissionDomainRepository), typeof(PermissionRepository));

        }

        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }
    }
}
