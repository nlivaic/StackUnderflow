using System;

namespace StackUnderflow.Core.Models
{
    public class UserCreateModel
    {
        public string Email { get; private set; }
        public Uri WebsiteUrl { get; private set; }
        public string AboutMe { get; private set; }
    }
}
