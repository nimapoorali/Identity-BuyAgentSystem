using FluentResults;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Users
{
    public class UserRole : Entity
    {
        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }
        public DateTimeP AssignDate { get; private set; }
        public ActivityState ActivityState { get; private set; }

        private UserRole()
        {

        }

        private UserRole(Guid userId, Guid roleId, DateTimeP assignDate, ActivityState activityState)
        {
            UserId = userId;
            RoleId = roleId;
            AssignDate = assignDate;
            ActivityState = activityState;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not UserRole)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            UserRole item = (UserRole)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return (item.UserId == this.UserId && item.RoleId == this.RoleId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //public static UserRole Create(Guid userId, Guid roleId, DateTimeP assignDate, int activityState)
        public static UserRole Create(Guid userId, Guid roleId, ActivityState activityState)
        {
            CheckRule(new FieldValueIsRequiredRule(userId, IdentityDataDictionary.UserId));
            CheckRule(new FieldValueIsRequiredRule(roleId, IdentityDataDictionary.RoleId));
            //CheckRule(new FieldValueIsRequiredRule(assignDate.ToString(), IdentityDataDictionary.AssignDate));
            //CheckRule(new DateMustNotBeFutureRule(assignDate, IdentityDataDictionary.AssignDate));
            //var activityStateResult = Enumeration.FromValue<ActivityState>(activityState);

            return new UserRole(userId, roleId, DateTimeP.Create(DateTime.Now), activityState);
        }
    }
}
