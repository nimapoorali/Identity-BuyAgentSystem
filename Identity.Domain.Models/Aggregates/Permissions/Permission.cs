using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using Identity.Domain.Models.Aggregates.Permissions.Events;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NP.Resources;

namespace Identity.Domain.Models.Aggregates.Permissions
{
    public class Permission : AggregateRoot
    {
        public PermissionName Name { get; private set; }
        public ActivityState ActivityState { get; private set; }

        private readonly List<PermissionRole> _roles = new();
        public IReadOnlyList<PermissionRole> Roles => _roles;


        private Permission() : base()
        {

        }

        private Permission(PermissionName name, ActivityState activityState) : this()
        {
            Name = name;
            ActivityState = activityState;
        }

        public static Permission Create(PermissionName name, ActivityState activityState)
        {
            DomainEvent.Raise(new PermissionNameUniquenessCheckRequested(name)).GetAwaiter().GetResult();

            return new Permission(name, activityState);
        }

        public void Change(PermissionName name, ActivityState activityState)
        {
            DomainEvent.Raise(new PermissionNameUniquenessCheckRequested(name, Id)).GetAwaiter().GetResult();

            Name = name;
            ActivityState = activityState;
        }

        public void Activate()
        {
            CheckRule(
                new IsAlreadyInStateRule(ActivityState.Value,
                                         ActivityState.Active.Value,
                                         IdentityDataDictionary.ActivityState));

            ActivityState = ActivityState.Active;
        }
        public void Deactivate()
        {
            CheckRule(
               new IsAlreadyInStateRule(ActivityState.Value,
                                        ActivityState.Deactive.Value,
                                        IdentityDataDictionary.ActivityState));

            ActivityState = ActivityState.Deactive;
        }
        public void Suspend()
        {
            CheckRule(
               new IsAlreadyInStateRule(ActivityState.Value,
                                        ActivityState.Suspend.Value,
                                        IdentityDataDictionary.ActivityState));

            ActivityState = ActivityState.Suspend;
        }

        public void AddRole(PermissionRole rolePermission)
        {
            if (rolePermission.PermissionId != Id)
                throw new BusinessRuleValidationException(Validations.InvalidRecord);

            if (Roles.Contains(rolePermission))
                //throw new BusinessRuleValidationException(IdentityValidations.DuplicatePermission);
                return;

            _roles.Add(rolePermission);

            return;

        }

        public void RemoveRole(PermissionRole rolePermission)
        {
            if (rolePermission.PermissionId != Id)
                throw new BusinessRuleValidationException(Validations.InvalidRecord);

            if (!Roles.Contains(rolePermission))
                throw new BusinessRuleValidationException(IdentityValidations.NotExistsPermissionForRole);

            _roles.Remove(rolePermission);

            return;

        }
    }
}
