using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SeedWork
{
    public interface IEntity
    {
        public Guid Id { get; }
        IReadOnlyList<IDomainEvent>? DomainEvents { get; }
    }
}
