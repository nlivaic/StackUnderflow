using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Tests.Builders
{
    public class TagBuilder
    {
        public List<Tag> Build(int tagCount) =>
            Builder<Tag>
                .CreateListOfSize(tagCount)
                .Build()
                .ToList();
    }
}
