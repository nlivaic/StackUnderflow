﻿using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Infrastructure.Caching;
using System.Threading.Tasks;

namespace StackUnderflow.Core.Services
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