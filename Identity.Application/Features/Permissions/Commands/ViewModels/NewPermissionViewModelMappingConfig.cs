using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Roles;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Commands.ViewModels
{
    public class NewPermissionViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Permission, NewPermissionViewModel>()
                .Map(dest => dest.Id, src => src.Id.ToString())
                .Map(dest => dest.ActivityState, src => src.ActivityState.ToString())
                .Map(dest => dest.Name, src => src.Name.Name);
        }
    }
}
