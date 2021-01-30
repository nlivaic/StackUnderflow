using System;
using StackUnderflow.Common.Base;

namespace StackUnderflow.Core.Entities
{
    public class UserRole : BaseEntity<Guid>
    {
        public Guid UserId { get; private set; }
        public Role Role { get; private set; }

        public UserRole(Guid userId, Role role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
