using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Permissions
{
    public class PermissionRole : Entity
    {
        public Guid RoleId { get; private set; }
        public Guid PermissionId { get; private set; }
        public DateTimeP AssignDate { get; private set; }
        public ActivityState ActivityState { get; private set; }

        private PermissionRole()
        {

        }

        private PermissionRole(Guid roleId, Guid permissionId, DateTimeP assignDate, ActivityState activityState)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            AssignDate = assignDate;
            ActivityState = activityState;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not PermissionRole)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            PermissionRole item = (PermissionRole)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return (item.RoleId == this.RoleId && item.PermissionId == this.PermissionId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static PermissionRole Create(Guid roleId, Guid permissionId, ActivityState activityState)
        {
            CheckRule(new FieldValueIsRequiredRule(roleId, IdentityDataDictionary.RoleId));
            CheckRule(new FieldValueIsRequiredRule(permissionId, IdentityDataDictionary.PermissionId));
            //CheckRule(new FieldValueIsRequiredRule(assignDate.ToString(), IdentityDataDictionary.AssignDate));
            //CheckRule(new DateMustNotBeFutureRule(assignDate, IdentityDataDictionary.AssignDate));
            //var activityStateResult = Enumeration.FromValue<ActivityState>(activityState);

            return new PermissionRole(roleId, permissionId, DateTimeP.Create(DateTime.Now), activityState);
        }
    }
}
