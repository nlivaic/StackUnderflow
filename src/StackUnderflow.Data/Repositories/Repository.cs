using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Interfaces;

namespace StackUnderflow.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity<Guid>
    {
        protected readonly StackUnderflowDbContext _context;

        public Repository(StackUnderflowDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(Guid id, bool isTracked = true)
        {
            var q = _context.Set<T>() as IQueryable<T>;
            if (!isTracked)
                q = q.AsNoTracking();
            return await q.SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool isTracked = true)
        {
            var q = _context.Set<T>() as IQueryable<T>;
            if (!isTracked)
                q = q.AsNoTracking();
            return await q.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> ListAllAsync(Expression<Func<T, bool>> predicate = null, bool isTracked = true)
        {
            var q = _context.Set<T>().AsQueryable();
            if (predicate != null)
                q = q.Where(predicate);
            if (!isTracked)
                q = q.AsNoTracking();
            return await q.ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id) =>
            await _context.Set<T>().AnyAsync(x => x.Id == id);

        public async Task<bool> ExistsAsync(IEnumerable<Guid> ids) =>
            (await _context.Set<T>().CountAsync(x => ids.Contains(x.Id)) == ids.Count());

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
