using System;
using StackUnderflow.Common.Base;

namespace StackUnderflow.Core.Entities
{
    public class UserRole : BaseEntity<int>
    {
        public UserRole(Guid userId, Role role)
        {
            UserId = userId;
            Role = role;
        }

        public Guid UserId { get; private set; }
        public Role Role { get; private set; }
    }
}
