using Identity.Domain.Models.Aggregates.Users;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Users.Commands.ViewModels
{
    public class UserRolesViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<IReadOnlyList<UserRole>, UserRolesViewModel>()
                .Map(dest => dest.UserId, src => src.First().UserId)
                .Map(dest => dest.RoleIds, src => src.Select(ur => ur.RoleId).ToArray());
        }
    }
}
