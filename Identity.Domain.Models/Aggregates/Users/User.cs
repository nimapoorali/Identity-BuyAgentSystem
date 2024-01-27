using FluentResults;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.Events;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Users
{
    public class User : AggregateRoot
    {
        public const PasswordStrength MinimumPasswordStrength = PasswordStrength.VeryStrong;
        public const int MobileKeyExpirationMinutes = 20;
        public const int EmailKeyExpirationMinutes = 20;

        public Username Username { get; private set; }
        public Password Password { get; private set; }
        public Name? NickName { get; private set; }
        public Name? FirstName { get; private set; }
        public Name? LastName { get; private set; }
        public Mobile? Mobile { get; private set; }
        public Email? Email { get; private set; }
        public ActivityState ActivityState { get; private set; }
        public bool IsActive { get { return ActivityState == ActivityState.Active; } }

        private readonly List<UserRole> _roles = new();
        public IReadOnlyList<UserRole> Roles => _roles;

        private User() : base()
        {
        }
        private User(Username username,
                     Password password,
                     Name? nickName,
                     Name? firstName,
                     Name? lastName,
                     Mobile? mobile,
                     Email? email,
                     ActivityState activityState) : this()
        {
            Username = username;
            Password = password;
            NickName = nickName;
            FirstName = firstName;
            LastName = lastName;
            Mobile = mobile;
            Email = email;
            ActivityState = activityState;
        }

        //private void BuildUserRoles(List<Guid> roleIds)
        //{
        //    roleIds.ForEach(roleId =>
        //    {
        //        var userRoleResult = UserRole.Create(Id, roleId, DateTime.Now, ActivityState.Deactive.Value);
        //        if (userRoleResult.IsSuccess)
        //            _roles.Add(userRoleResult.Value);
        //    });
        //}

        public static User Create(Username username,
                                  Password password,
                                  Name? nickName,
                                  Name? firstName,
                                  Name? lastName,
                                  Mobile? mobile,
                                  Email? email,
                                  ActivityState activityState)
        {
            if (!password.IsHashed)
            {
                throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.PasswordNotHashed));
            }

            DomainEvent.Raise(new UsernameUniquenessCheckRequested(username)).GetAwaiter().GetResult();

            return new User(username,
                            password,
                            nickName,
                            firstName,
                            lastName,
                            mobile,
                            email,
                            activityState);
        }

        public void Chagne(Name? nickName, Name? firstName, Name? lastName, ActivityState activityState)
        {
            NickName = nickName;
            FirstName = firstName;
            LastName = lastName;
            ActivityState = activityState;
        }

        public void ChangeMobile(Mobile mobile)
        {
            Mobile = mobile;
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }

        public void DeleteMobile()
        {
            Mobile = null;
        }

        public void DeleteEmail()
        {
            Email = null;
        }

        public void ActivateByMobile(string? verificationKey)
        {
            VerifyMobile(verificationKey);

            if (Mobile!.IsVerified)
            {
                ActivityState = ActivityState.Active;
            }
        }

        public void VerifyMobile(string? verificationKey)
        {
            if (Mobile is null)
                throw new BusinessRuleValidationException(IdentityValidations.NoMobileSet);

            Mobile = Mobile.Verify(verificationKey);
        }

        public void ActivateByEmail(string? verificationKey)
        {
            VerifyEmail(verificationKey);

            if (Email!.IsVerified)
            {
                ActivityState = ActivityState.Active;
            }
        }

        public void VerifyEmail(string? verificationKey)
        {
            if (Email is null)
                throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

            Email = Email.Verify(verificationKey);
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

        public void AddRole(UserRole userRole)
        {
            if (userRole.UserId != Id)
                throw new BusinessRuleValidationException(Validations.InvalidRecord);

            if (Roles.Contains(userRole))
                //throw new BusinessRuleValidationException(IdentityValidations.DuplicateRole);
                return;

            _roles.Add(userRole);

            return;

        }

        public void RemoveRole(UserRole userRole)
        {
            if (userRole.UserId != Id)
                throw new BusinessRuleValidationException(Validations.InvalidRecord);

            if (!Roles.Contains(userRole))
                throw new BusinessRuleValidationException(IdentityValidations.NotExistsRoleForUser);

            _roles.Remove(userRole);

            return;

        }
    }
}
