using Identity.Domain.Models.Aggregates.Users;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.ViewModels
{
    public class NewUserViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<User, NewUserViewModel>()
                .Map(dest => dest.Id, src => src.Id.ToString())
                .Map(dest => dest.Username, src => src.Username.Value)
                .Map(dest => dest.FirstName, src => src.FirstName!.Value)
                .Map(dest => dest.LastName, src => src.LastName!.Value)
                .Map(dest => dest.NickName, src => src.NickName!.Value)
                .Map(dest => dest.Mobile, src => src.Mobile!.Value)
                .Map(dest => dest.Email, src => src.Email!.Value)
                .Map(dest => dest.ActivityState, src => src.ActivityState.ToString())
                //.Map(dest => dest.MobileVerificationKey, src => src.Mobile!.VerificationKey)
                //.Map(dest => dest.EmailVerificationKey, src => src.Email!.VerificationKey)
                ;
        }
    }
}
