using Identity.Application;
using Identity.Infrastructure;
//using Identity.Domain.Models;
//using Identity.Domain.Services;
//using Identity.Infrastructure;
using Identity.Infrastructure.Persistence;

namespace Identity.GrpcServer
{
    public static class ServicesSetup
    {
        public static void AddIdentityGrpcApi(this IServiceCollection services, IConfiguration configuration)
        {
            //Add infrastructures
            services.AddIdentityInfrastructure(configuration);

            //Add repositories
            services.AddIdentityPersistenceInfrastructure(configuration);

            //Add application layer services dependencies
            services.RegisterApplicationServices();

        }
    }
}
