using FluentResults;
using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Abstraction.Users;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel.Rules;
using Identity.Resources;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Roles.Events
{
    public class UsernameUniquenessCheckHandler : INotificationHandler<UsernameUniquenessCheckRequested>
    {
        public IUserDomainRepository UserDomainRepository { get; }

        public UsernameUniquenessCheckHandler(IUserDomainRepository userDomainRepository)
        {
            UserDomainRepository = userDomainRepository;
        }

        public async Task Handle(UsernameUniquenessCheckRequested notification, CancellationToken cancellationToken)
        {
            var usernameAlreadyExists = false;

            if (notification.CurrentUserId is null)
                usernameAlreadyExists =
                    await UserDomainRepository.IsUsernameExists(notification.Username, cancellationToken);
            else
                usernameAlreadyExists =
                    await UserDomainRepository.IsUsernameExists(notification.Username, notification.CurrentUserId.Value, cancellationToken);

            if (usernameAlreadyExists)
                throw new BusinessRuleValidationException(new BrokenBusinessRule(
                    string.Format(Validations.AlreadyExistsValueForField,
                                  notification.Username.Value,
                                  IdentityDataDictionary.Username)));
        }
    }
}
