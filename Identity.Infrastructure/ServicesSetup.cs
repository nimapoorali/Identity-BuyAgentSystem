using Identity.Application.Abstraction;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure
{
    public static class ServicesSetup
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterJWT(configuration);
        }

        private static void RegisterJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITokenService, JWTUtility>();
            //services.AddSingleton<ITokenService>(x => new JWTUtility(secret, timeout, issuer, audience));

            var tokenGeneration = configuration["Identity:TokenGeneration"];
            if (tokenGeneration == "internal")
            {
                var secret = configuration["Jwt:Secret"];
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];


                services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
            }
            else
            {
                services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            }

        }
    }
}
