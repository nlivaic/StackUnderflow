using System;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface IOwneable
    {
        Guid UserId { get; }
        User User { get; }

        bool CanBeEditedBy(User editingUser);
    }
}