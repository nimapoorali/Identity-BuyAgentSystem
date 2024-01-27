using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Users;
using Identity.Domain.Models.Aggregates.Users;
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

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        public IIdentityUnitOfWork UnitOfWork { get; }

        public UserService(IIdentityUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<User?> GetUser(string username, string password)
        {
            var usernameObject = Username.Create(username!);

            var user = await UnitOfWork.Users.SingleAsync(user => user.Username == usernameObject);

            if (user is null)
                throw new BusinessRuleValidationException(new BrokenBusinessRule(Validations.NotExistsRecord));

            if (!user.IsActive)
                throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.NotActiveUser));

            if (!user.Password.Validate(password))
                throw new BusinessRuleValidationException(new BrokenBusinessRule(IdentityValidations.IncorrectUsernameOrPasseword));

            return user;
        }

        public async Task<User?> GetMobileUser(string mobile, string key)
        {
            var username = Username.Create(mobile);

            var mobileUser = await UnitOfWork.Users.SingleAsync(u => u.Username == username);

            if (mobileUser is null)
                throw new BusinessRuleValidationException(Validations.NotExistsRecord);

            if (!mobileUser.IsActive)
                throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

            if (mobileUser.Mobile is null)
                throw new BusinessRuleValidationException(IdentityValidations.NoMobileSet);

            if (!mobileUser.Mobile.IsVerified)
                throw new BusinessRuleValidationException(IdentityValidations.MobileNotVerified);

            if (mobileUser.Mobile.KeyExpirationDate is not null && mobileUser.Mobile.KeyExpirationDate < DateTimeP.Now)
                throw new BusinessRuleValidationException(Validations.ExpiredVerificationKey);

            if (mobileUser.Mobile.VerificationKey != key)
                throw new BusinessRuleValidationException(Validations.InvalidVerificationKey);

            return mobileUser;
        }

        public async Task<User?> GetEmailUser(string email, string key)
        {
            var emailUsernameObject = Username.Create(email);

            var emailUser = await UnitOfWork.Users.SingleAsync(user => user.Username == emailUsernameObject);

            if (emailUser is null)
                throw new BusinessRuleValidationException(Validations.NotExistsRecord);

            if (!emailUser.IsActive)
                throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

            if (emailUser.Email is null)
                throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

            if (!emailUser.Email.IsVerified)
                throw new BusinessRuleValidationException(IdentityValidations.EmailNotVerified);

            if (emailUser.Email.KeyExpirationDate is not null && emailUser.Email.KeyExpirationDate < DateTimeP.Now)
                throw new BusinessRuleValidationException(Validations.ExpiredVerificationKey);

            if (emailUser.Email.VerificationKey != key)
                throw new BusinessRuleValidationException(Validations.InvalidVerificationKey);

            return emailUser;
        }

        public async Task NewMobileUserVerificationKey(string mobile)
        {
            var mobileUsernameObject = Username.Create(mobile);

            var mobileUser = await UnitOfWork.Users.SingleAsync(user => user.Username == mobileUsernameObject);

            if (mobileUser is null)
                throw new BusinessRuleValidationException(Validations.NotExistsRecord);

            if (!mobileUser.IsActive)
                throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

            if (mobileUser.Mobile is null)
                throw new BusinessRuleValidationException(IdentityValidations.NoMobileSet);

            if (!mobileUser.Mobile.IsVerified)
                throw new BusinessRuleValidationException(IdentityValidations.MobileNotVerified);

            var verificationKey = StringUtil.RandomNumeric(5);
            var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(2));
            var mobileObject = Mobile.Create(mobileUser.Mobile.Value, mobileUser.Mobile.IsVerified, verificationKey, keyExpirationDate);

            mobileUser.ChangeMobile(mobileObject);

            UnitOfWork.Users.Update(mobileUser);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task NewEmailUserVerificationKey(string email)
        {
            var emailUsernameObject = Username.Create(email);

            var emailUser = await UnitOfWork.Users.SingleAsync(user => user.Username == emailUsernameObject);

            if (emailUser is null)
                throw new BusinessRuleValidationException(Validations.NotExistsRecord);

            if (!emailUser.IsActive)
                throw new BusinessRuleValidationException(IdentityValidations.NotActiveUser);

            if (emailUser.Email is null)
                throw new BusinessRuleValidationException(IdentityValidations.NoEmailSet);

            if (!emailUser.Email.IsVerified)
                throw new BusinessRuleValidationException(IdentityValidations.EmailNotVerified);

            var verificationKey = Guid.NewGuid().ToString();
            var keyExpirationDate = DateTimeP.Create(DateTime.Now.AddMinutes(10));
            var emailObject = Email.Create(emailUser.Email!.Value, emailUser.Email!.IsVerified, verificationKey, keyExpirationDate);

            emailUser.ChangeEmail(emailObject);

            UnitOfWork.Users.Update(emailUser);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
