using Identity.Domain.Models.Aggregates.Permissions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Queries.ViewModels
{
    public class PermissionViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ActivityState { get; set; }
        public Guid[]? RoleIds { get; set; }
    }

    public class PermissionViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Permission, PermissionViewModel>()
                .Map(dest => dest.Id, src => src.Id.ToString())
                .Map(dest => dest.ActivityState, src => src.ActivityState.ToString())
                .Map(dest => dest.Name, src => src.Name.Name)
                .Map(dest => dest.RoleIds, src => src.Roles.Select(r => r.RoleId).ToArray());
        }
    }
}
