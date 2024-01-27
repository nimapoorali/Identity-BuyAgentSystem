using FluentResults;
using Identity.Domain.Models.Abstraction.Roles;
using NP.Shared.Domain.Models.SeedWork;
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
    public class RoleTitleUniquenessCheckHandler : INotificationHandler<RoleTitleUniquenessCheckRequested>
    {
        public IRoleDomainRepository RoleDomainRepository { get; }

        public RoleTitleUniquenessCheckHandler(IRoleDomainRepository roleDomainRepository)
        {
            RoleDomainRepository = roleDomainRepository;
        }

        public async Task Handle(RoleTitleUniquenessCheckRequested notification, CancellationToken cancellationToken)
        {
            var titleAlreadyExists = false;

            if (notification.CurrentRoleId is null)
                titleAlreadyExists =
                    await RoleDomainRepository.IsTitleExists(notification.RoleTitle, cancellationToken);
            else
                titleAlreadyExists =
                    await RoleDomainRepository.IsTitleExists(notification.RoleTitle, notification.CurrentRoleId.Value, cancellationToken);

            if (titleAlreadyExists)
                throw new BusinessRuleValidationException(
                    string.Format(Validations.AlreadyExistsValueForField,
                                  notification.RoleTitle.Title,
                                  IdentityDataDictionary.RoleTitle));
        }
    }
}
