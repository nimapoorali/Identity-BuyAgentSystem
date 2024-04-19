using Identity.Domain.Models.Abstraction.Permissions;
using NP.Shared.Domain.Models.SeedWork;
using Identity.Resources;
using MediatR;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.Aggregates.Permissions.Events
{
    public class PermissionNameUniquenessCheckHandler : INotificationHandler<PermissionNameUniquenessCheckRequested>
    {
        public IPermissionDomainRepository PermissionDomainRepository { get; }

        public PermissionNameUniquenessCheckHandler(IPermissionDomainRepository permissionDomainRepository)
        {
            PermissionDomainRepository = permissionDomainRepository;
        }

        public async Task Handle(PermissionNameUniquenessCheckRequested notification, CancellationToken cancellationToken)
        {
            var nameAlreadyExists = false;

            if (notification.CurrentPermissionId is null)
                nameAlreadyExists =
                    await PermissionDomainRepository.IsNameExists(notification.Name, cancellationToken);
            else
                nameAlreadyExists =
                    await PermissionDomainRepository.IsNameExists(notification.Name, notification.CurrentPermissionId.Value, cancellationToken);

            if (nameAlreadyExists)
                throw new BusinessRuleValidationException(
                    string.Format(Validations.AlreadyExistsValueForField,
                                  notification.Name.Name,
                                  SharedDataDictionary.Name));
        }
    }
}
