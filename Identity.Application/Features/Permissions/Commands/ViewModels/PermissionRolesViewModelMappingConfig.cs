using Identity.Domain.Models.Aggregates.Permissions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Commands.ViewModels
{
    public class PermissionRolesViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<IReadOnlyList<PermissionRole>, PermissionRolesViewModel>()
                .Map(dest => dest.PermissionId, src => src.First().PermissionId)
                .Map(dest => dest.RoleIds, src => src.Select(ur => ur.RoleId).ToArray());
        }
    }
}
