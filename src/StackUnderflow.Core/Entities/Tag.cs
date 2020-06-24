using StackUnderflow.Common.Base;
using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Entities
{
    public class Tag : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public IEnumerable<QuestionTag> QuestionTags { get; private set; }

        private Tag()
        { }

        public Tag(string name)
        {
            Name = name;
        }
    }
}