using Identity.Application.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Authorizations.Commands.ViewModels
{
    public class TokenViewModelMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Token, TokenViewModel>()
                .Map(dest => dest.Token, src => src.Value)
                .Map(dest => dest.ExpireTime, src => src.ExpiresAt.ToString());// "yyyy/MM/dd HH:mm:ss"););
        }
    }
}
