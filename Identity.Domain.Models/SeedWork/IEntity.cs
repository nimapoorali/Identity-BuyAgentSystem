using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Models.SeedWork
{
    public interface IEntity
    {
        public Guid Id { get; }
        IReadOnlyList<IDomainEvent>? DomainEvents { get; }
    }
}
