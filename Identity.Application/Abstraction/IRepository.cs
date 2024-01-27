using NP.Shared.Domain.Models.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IAggregateRoot
    {
        void AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        void Update(TEntity entity, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
        //void RemoveById(Guid id);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
