using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Data.Repositories
{
    public class LimitsRepository : ILimitsRepository
    {
        private readonly StackUnderflowDbContext _context;

        public LimitsRepository(StackUnderflowDbContext context)
        {
            _context = context;
        }

        public async Task<Limits> GetAsync()
        {
            var limitsKeyValues = await _context.LimitsKeyValuePairs.ToListAsync();
            var limits = Limits.Create();
            foreach (var limit in limitsKeyValues)
            {
                var propertyInfo = typeof(Limits).GetProperty(limit.LimitKey);
                if (propertyInfo == null)
                {
                    throw new LimitNotMappable($"{nameof(Limits)} property {limit.LimitKey} not found, even though it is defined in database.");
                }
                if (propertyInfo.PropertyType == typeof(TimeSpan))
                {
                    propertyInfo.SetValue(limits, new TimeSpan(0, limit.LimitValue, 0));
                }
                else
                {
                    propertyInfo.SetValue(limits, limit.LimitValue);
                }
            }
            return limits;
        }
    }
}
