using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class User : BaseEntity<Guid>
    {
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

        private List<UserRole> _roles = new List<UserRole>();

        private User()
        {
        }

        public void Edit(string websiteUrl, string aboutMe, BaseLimits limits)
        {
            Validate(websiteUrl, aboutMe, limits);
            WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? null
                : new Uri(websiteUrl);
            AboutMe = aboutMe;
            CreatedOn = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }

        /// <summary>
        /// Provide Id based on token service's user identifier.
        /// </summary>
        public static User Create(BaseLimits limits, Guid id, string username, string email = null, string websiteUrl = null, string aboutMe = null)
        {
            Validate(websiteUrl, aboutMe, limits);
            User user = new User();
            user.Id = id;
            user.Username = username;
            user.Email = email;
            user.WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? null
                : new Uri(websiteUrl);
            user.AboutMe = aboutMe;
            user.CreatedOn = DateTime.UtcNow;
            user.SeenNow();
            user.AssignRole(Role.User);
            return user;
        }

        public void AssignRole(Role role)
        {
            if (_roles.Any(r => r.Role == role))
            {
                throw new BusinessException($"Role {role.ToString()} already assigned to user.");
            }
            _roles.Add(new UserRole(Id, role));
        }

        public void SeenNow() =>
            LastSeen = DateTime.UtcNow;

        public int ApplyVoteToPoint(VoteTypeEnum voteType) =>
            voteType switch
            {
                VoteTypeEnum.Upvote => Points++,
                VoteTypeEnum.Downvote => Points--,
                _ => throw new ArgumentException("Unknown vote type during point recalculation.")
            };

        public void PointDown() => Points--;

        private static void Validate(string websiteUrl, string aboutMe, BaseLimits limits)
        {
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
