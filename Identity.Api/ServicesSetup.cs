using Identity.Application;
using Identity.Domain.Models;
using Identity.Domain.Services;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistence;

namespace Identity.Api
{
    public static class ServicesSetup
    {
        public static void AddIdentityApi(this IServiceCollection services, IConfiguration configuration)
        {
            //Add infrastructures
            services.AddIdentityInfrastructure(configuration);

            //Add repositories
            services.AddIdentityPersistenceInfrastructure(configuration);

            //Add application layer dependencies
            services.AddIdentityApplication(configuration);

            //Add domain layer dependencies
            services.AddIdentityDomain();

        }
    }
}
