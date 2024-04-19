using Identity.Application.Abstraction;
using NP.Shared.Domain.Models.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class, IAggregateRoot
    {
        public IdentityDbContext Context { get; }

        public ReadOnlyRepository(IdentityDbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().Where(expression).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().FindAsync(new object?[] { id }, cancellationToken);
        }

        public async Task<bool> IsExistByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().AnyAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<T?> SingleAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().SingleOrDefaultAsync(expression, cancellationToken: cancellationToken);
        }
    }
}
