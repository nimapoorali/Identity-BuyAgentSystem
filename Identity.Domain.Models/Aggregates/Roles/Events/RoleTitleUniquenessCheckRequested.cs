using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Roles.Events
{
    public class RoleTitleUniquenessCheckRequested : INotification
    {
        public RoleTitle RoleTitle { get; }
        public Guid? CurrentRoleId { get; set; }
        public RoleTitleUniquenessCheckRequested(RoleTitle roleTitle, Guid? currentRoleId = null)
        {
            RoleTitle = roleTitle;
            CurrentRoleId = currentRoleId;
        }
    }
}
