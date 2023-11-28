using Identity.Application.Features.Roles.Queries.ViewModels;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Queries.ViewModels
{
    public class UserViewModel
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public bool? IsMobileVerified { get; set; }
        public string? Email { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string? ActivityState { get; set; }
    }

    public class UserViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<User, UserViewModel>()
                .Map(dest => dest.Id, src => src.Id.ToString())
                .Map(dest => dest.ActivityState, src => src.ActivityState.ToString())
                .Map(dest => dest.Username, src => src.Username.Value)
                .Map(dest => dest.FirstName, src => src.FirstName.Value)
                .Map(dest => dest.LastName, src => src.LastName.Value)
                .Map(dest => dest.NickName, src => src.NickName.Value)
                .Map(dest => dest.Email, src => src.Email.Value)
                .Map(dest => dest.IsEmailVerified, src => src.Email.IsVerified)
                .Map(dest => dest.Mobile, src => src.Mobile.Value)
                .Map(dest => dest.IsMobileVerified, src => src.Mobile.IsVerified);
        }
    }
}
