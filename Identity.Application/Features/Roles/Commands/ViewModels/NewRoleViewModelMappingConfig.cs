using Identity.Domain.Models.Aggregates.Roles;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Roles.Commands.ViewModels
{
    public class NewRoleViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Role, NewRoleViewModel>()
                .Map(dest => dest.Id, src => src.Id.ToString())
                .Map(dest => dest.ActivityState, src => src.ActivityState.ToString())
                .Map(dest => dest.GroupTitle, src => src.GroupTitle.Title)
                .Map(dest => dest.Title, src => src.Title.Title);
        }
    }
}
