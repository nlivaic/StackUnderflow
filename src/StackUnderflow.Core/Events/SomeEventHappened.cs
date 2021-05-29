using System;

namespace StackUnderflow.Core.Events
{
    public interface SomeEventHappened
    {
        public Guid Id { get; set; }
    }
}
