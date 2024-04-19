using Identity.Application;
using Identity.Application.Abstraction;
using Identity.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure
{
    public class JWTUtility : ITokenService
    {
        public string Secret { get; }
        public int Timeout { get; }
        public string Issuer { get; }
        public string Audience { get; }

        public JWTUtility(IConfiguration configuration)//string secret, int timeout, string issuer, string audience)
        {
            var tokenGeneration = configuration["Identity:TokenGeneration"];
            if (tokenGeneration == "internal")
            {
                Secret = configuration["Jwt:Secret"];
                Issuer = configuration["Jwt:Issuer"];
                Audience = configuration["Jwt:Audience"];
                Timeout = int.Parse(configuration["Jwt:TimeoutInMinutes"]);
            }
            else
            {
                Secret = "";
                Timeout = 0;
                Issuer = "";
                Audience = "";
            }
        }

        public Task<Token> GenerateToken(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("id", userId.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(Timeout),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var generatedToken = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(generatedToken);

            var token = new Token()
            {
                Value = tokenString,
                ExpiresAt = tokenDescriptor.Expires.Value,
            };

            return Task.FromResult(token);
        }

        public Task<string> ValidateToken(string token)
        {
            if (token == null)
                return Task.FromResult("");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    //ClockSkew = TimeSpan.Zero,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // return user id from JWT token if validation successful
                return Task.FromResult(userId);
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return Task.FromResult("");
            }
        }
    }
}