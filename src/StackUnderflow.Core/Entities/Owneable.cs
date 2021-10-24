using System;
using System.Linq;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Owneable : IOwneable
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public bool CanBeEditedBy(User editingUser) =>
            UserId == editingUser.Id || editingUser.Roles.Any(r => r.Role == Role.Moderator);
    }
}
