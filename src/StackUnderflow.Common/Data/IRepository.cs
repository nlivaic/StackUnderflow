using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using StackUnderflow.Common.Base;

namespace StackUnderflow.Common.Query
{
    public interface IRepository<T> where T : BaseEntity<Guid>
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> ListAllAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null/*ISpecification<T> spec*/);
        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null);
    }
}