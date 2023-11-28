using Identity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction
{
    public interface ITokenService
    {
        Task<Token> GenerateToken(Guid userId);
        Task<string> ValidateToken(string token);
    }
}
