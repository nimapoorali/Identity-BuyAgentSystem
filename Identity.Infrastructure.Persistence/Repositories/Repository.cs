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
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        //public IdentityDbContext Context { get; }

        public Repository(IdentityDbContext dbContext) : base(dbContext)
        {
            //Context = dbContext;
        }

        #region ReadOnly
        //public async Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken = default)
        //{
        //    return await Context.Set<T>().ToListAsync(cancellationToken);
        //}

        //public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        //{
        //    return await Context.Set<T>().Where(expression).ToListAsync(cancellationToken);
        //}

        //public async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        //{
        //    return await Context.Set<T>().FindAsync(new object?[] { id }, cancellationToken);
        //}

        //public async Task<bool> IsExistByIdAsync(Guid id, CancellationToken cancellationToken = default)
        //{
        //    return await Context.Set<T>().AnyAsync(t => t.Id == id, cancellationToken);
        //}
        #endregion

        public async void AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await Context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public async void AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await Context.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity, CancellationToken cancellationToken = default)
        {
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

    }
}
