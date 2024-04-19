using NP.Shared.Domain.Models.SeedWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models
{
    public static class ServicesSetup
    {
        public static void AddIdentityDomain(this IServiceCollection services)
        {
            services.RegisterMidatR();
            services.RegisterDomainEvents();
        }

        private static void RegisterMidatR(this IServiceCollection services)
        {
            services.AddMediatR(m =>
            {
                m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }

        private static void RegisterDomainEvents(this IServiceCollection services)
        {
            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
            DomainEvent.Mediator = () => ServiceLocator.Current.GetInstance<IMediator>();

        }
    }
}
