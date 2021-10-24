using System;
using System.Collections.Generic;
using StackUnderflow.Common.Base;

namespace StackUnderflow.Core.Entities
{
    public class Tag : BaseEntity<Guid>
    {
        public Tag(string name)
        {
            Name = name;
        }

        private Tag()
        {
        }

        public string Name { get; private set; }
        public IEnumerable<QuestionTag> QuestionTags { get; private set; }
    }
}