using StackUnderflow.Common.Base;
using System;

namespace StackUnderflow.Core.Models
{
    public class Tag : BaseEntity<Guid>
    {
        public string Name { get; private set; }

        public Tag(string name)
        {
            Name = name;
        }
    }
}