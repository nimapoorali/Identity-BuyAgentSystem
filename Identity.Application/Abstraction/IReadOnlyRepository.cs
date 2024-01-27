using NP.Shared.Domain.Models.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction
{
    public interface IReadOnlyRepository<TEntity> where TEntity : IAggregateRoot
    {
        Task<TEntity?> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> FindAll(CancellationToken cancellationToken = default);
        Task<bool> IsExistByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
