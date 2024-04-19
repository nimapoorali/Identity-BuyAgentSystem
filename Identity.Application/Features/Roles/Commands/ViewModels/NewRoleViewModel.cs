using FluentResults;
using Identity.Domain.Models.Aggregates.Roles;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.Roles.Commands.ViewModels
{
    public class NewRoleViewModel
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? GroupTitle { get; set; }
        public string? ActivityState { get; set; }
    }
}
