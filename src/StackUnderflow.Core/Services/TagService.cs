using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync(IEnumerable<Guid> tagIds)
        {
            // @nl: check generated SQL
            // IEnumerable<Tag> tags = await _tagRepository.ListAllAsync(t => tagIds.Contains(t.Id));
            var tags = await _tagRepository.ListAllAsync(t => tagIds.Contains(t.Id));
            var nonExistingTags = tagIds.Except(tags.Select(t => t.Id));
            if (nonExistingTags.Any())
            {
                throw new ArgumentException($"Tags '{string.Join(", ", nonExistingTags.Select(t => t.ToString()))}' do not exist.");
            }
            return tags;
        }
    }
}