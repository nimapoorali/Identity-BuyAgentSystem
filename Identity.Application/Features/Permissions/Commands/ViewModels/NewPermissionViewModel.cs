using FluentResults;
using Identity.Domain.Models.Aggregates.Roles;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Permissions.Commands.ViewModels
{
    public class NewPermissionViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? ActivityState { get; set; }
    }
}
