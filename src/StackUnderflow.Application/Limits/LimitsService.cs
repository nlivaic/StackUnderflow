using System.Threading.Tasks;
using SparkRoseDigital.Infrastructure.Caching;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Application.PointServices
{
    public class LimitsService : ILimitsService
    {
        private readonly ILimitsRepository _limitsRepository;
        private readonly ICache _cache;

        public LimitsService(ILimitsRepository limitsRepository, ICache cache)
        {
            _limitsRepository = limitsRepository;
            _cache = cache;
        }

        public async Task<Limits> Get() =>
            await _cache.GetOrCreateAsync("limits", async () => await _limitsRepository.GetAsync(), 600);
    }
}
