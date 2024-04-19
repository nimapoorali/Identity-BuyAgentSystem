using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Permissions.Events
{
    public class PermissionNameUniquenessCheckRequested : INotification
    {
        public PermissionName Name { get; }
        public Guid? CurrentPermissionId { get; set; }
        public PermissionNameUniquenessCheckRequested(PermissionName name, Guid? currentPermissionId = null)
        {
            Name = name;
            CurrentPermissionId = currentPermissionId;
        }
    }
}
