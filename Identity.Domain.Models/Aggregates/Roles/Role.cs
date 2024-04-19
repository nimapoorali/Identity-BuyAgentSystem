using FluentResults;
using Identity.Domain.Models.Aggregates.Roles.Events;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Roles
{
    public class Role : AggregateRoot
    {
        public RoleTitle Title { get; private set; }
        public RoleGroupTitle GroupTitle { get; private set; }
        public ActivityState ActivityState { get; private set; }


        private Role() : base()
        {

        }
        private Role(RoleTitle title, RoleGroupTitle groupTitle, ActivityState activityState) : this()
        {
            Title = title;
            GroupTitle = groupTitle;
            ActivityState = activityState;
        }

        public static Role Create(RoleTitle title, RoleGroupTitle groupTitle, ActivityState activityState)
        {

            DomainEvent.Raise(new RoleTitleUniquenessCheckRequested(title)).GetAwaiter().GetResult();

            return new Role(title, groupTitle, activityState);
        }

        public void Change(RoleTitle title, RoleGroupTitle groupTitle, ActivityState activityState)
        {

            DomainEvent.Raise(new RoleTitleUniquenessCheckRequested(title, Id)).GetAwaiter().GetResult();

            Title = title;
            GroupTitle = groupTitle;
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


    }
}
