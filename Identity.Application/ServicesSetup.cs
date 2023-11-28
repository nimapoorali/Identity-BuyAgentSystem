using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Application.Abstraction.Generals;
using Identity.Application.Abstraction.Permissions;
using Identity.Application.Abstraction.Users;
using Identity.Application.Features.Roles.Commands.ViewModels;
using Identity.Application.Features.Users.Commands.Validators;
using Identity.Application.Services;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application
{
    public static class ServicesSetup
    {
        public static void AddIdentityApplication(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterApplicationMappings();
            services.RegisterMidatR();
            services.RegisterValidators();
            services.RegisterConfigsOption(configuration);
            services.RegisterApplicationServices();
        }


        public static void RegisterApplicationMappings()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(NewRoleViewModelMappingConfig).Assembly);
        }

        private static void RegisterMidatR(this IServiceCollection services)
        {
            services.AddMediatR(m =>
            {
                m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }

        private static void RegisterValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ActivateUserByEmailCommandValidator>();
        }

        private static void RegisterConfigsOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<Configs>(configuration.GetSection("Configs"));
        }

        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>()
                    .AddScoped<IUserService, UserService>()
                    .AddScoped<ICachingService, CachingService>();
        }
    }
}
