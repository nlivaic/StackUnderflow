using System;

namespace StackUnderflow.Core.Models
{
    public class UserCreateModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
