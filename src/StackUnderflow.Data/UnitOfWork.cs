using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;

namespace StackUnderflow.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StackUnderflowDbContext _dbContext;

        public UnitOfWork(StackUnderflowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}