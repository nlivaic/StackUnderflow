using System;

namespace StackUnderflow.Application.Users.Models
{
    public class UserEditModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; private set; }
        public Uri WebsiteUrl { get; private set; }
        public string AboutMe { get; private set; }
    }
}
