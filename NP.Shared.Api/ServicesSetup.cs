using System.Reflection;

namespace NP.Shared.Api
{
    public static class ServicesSetup
    {
        public static void AddSharedApi(this IServiceCollection services)
        {
            services.RegisterMidatR();
        }

        private static void RegisterMidatR(this IServiceCollection services)
        {
            services.AddMediatR(m =>
            {
                m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }

    
}
