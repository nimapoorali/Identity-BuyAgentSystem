using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Roles.Events
{
    public class UsernameUniquenessCheckRequested : INotification
    {
        public Username Username { get; }
        public Guid? CurrentUserId { get; set; }
        public UsernameUniquenessCheckRequested(Username username, Guid? currentUserId = null)
        {
            Username = username;
            CurrentUserId = currentUserId;
        }
    }
}
