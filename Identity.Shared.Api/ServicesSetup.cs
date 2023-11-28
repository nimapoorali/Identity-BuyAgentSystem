using Identity.Infrastructure;
using NP.Shared.Api;

namespace Identity.Shared.Api
{
    public static class ServicesSetup
    {
        public static void AddIdentityApi(this IServiceCollection services, IConfiguration configuration)
        {
            //Add infrastructures
            services.AddIdentityInfrastructure(configuration);
            services.AddSharedApi();
            Application.ServicesSetup.RegisterApplicationMappings();
        }


    }
}
