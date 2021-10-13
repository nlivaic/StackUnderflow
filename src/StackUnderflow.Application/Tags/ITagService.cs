using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Application.Tags
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetTagsAsync(IEnumerable<Guid> tagIds);
    }
}