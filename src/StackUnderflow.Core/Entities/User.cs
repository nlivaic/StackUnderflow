using System;
using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Guards;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class User : BaseEntity<Guid>
    {
        private readonly List<UserRole> _roles = new ();

        private User()
        {
        }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public Uri WebsiteUrl { get; private set; }
        public string AboutMe { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int Points { get; private set; }
        public IEnumerable<UserRole> Roles => _roles;
        public IEnumerable<Question> Questions { get; private set; } = new List<Question>();
        public IEnumerable<Answer> Answers { get; private set; } = new List<Answer>();
        public IEnumerable<Comment> Comments { get; private set; } = new List<Comment>();

        public static User Create(ILimits limits, Guid id, string username, string email = null, string websiteUrl = null, string aboutMe = null)
        {
            Validate(username, websiteUrl, aboutMe, email, limits);
            var user = new User
            {
                Id = id,
                Username = username,
                Email = email,
                WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? null
                : new Uri(websiteUrl),
                AboutMe = aboutMe,
                CreatedOn = DateTime.UtcNow
            };
            user.SeenNow();
            user.AssignRole(Role.User);
            return user;
        }

        public void Edit(string websiteUrl, string aboutMe, ILimits limits)
        {
            Validate(Username, websiteUrl, aboutMe, Email, limits);
            WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? null
                : new Uri(websiteUrl);
            AboutMe = aboutMe;
            CreatedOn = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }

        public void AssignRole(Role role)
        {
            if (_roles.Any(r => r.Role == role))
            {
                throw new BusinessException($"Role {role} already assigned to user.");
            }
            _roles.Add(new UserRole(Id, role));
        }

        public void SeenNow() =>
            LastSeen = DateTime.UtcNow;

        public void PointDown() => Points--;

        private static void Validate(string username, string websiteUrl, string aboutMe, string email, ILimits limits)
        {
            if (string.IsNullOrWhiteSpace(username)
                || username.Length < limits.UsernameMinimumLength
                || username.Length > limits.UsernameMaximumLength)
            {
                throw new BusinessException("Username not valid.");
            }
            Guards.ValidEmail(email);
            if (!string.IsNullOrWhiteSpace(websiteUrl) && !Uri.TryCreate(websiteUrl, UriKind.Absolute, out var _))
            {
                throw new BusinessException("Website Url is not valid.");
            }
            if (!string.IsNullOrWhiteSpace(aboutMe) && aboutMe.Length > limits.AboutMeMaximumLength)
            {
                throw new BusinessException($"About Me is too long, must be at most {limits.AboutMeMaximumLength} characters.");
            }
        }
    }
}
