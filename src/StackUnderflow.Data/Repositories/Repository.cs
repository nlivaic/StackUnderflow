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

        public async Task<T> GetByIdAsync(Guid id) =>
            await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> ListAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            var q = _context.Set<T>().AsQueryable();
            if (predicate != null)
            {
                q = q.Where(predicate);
            }
            return await q.ToListAsync();
        }

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