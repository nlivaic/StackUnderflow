using System;
using System.Collections.Generic;
using System.Net.Mail;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Username { get; private set; }
        public MailAddress Email { get; private set; }
        public Uri WebsiteUrl { get; private set; }
        public string AboutMe { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime LastSeen { get; private set; }
        public IEnumerable<Question> Questions { get; private set; } = new List<Question>();
        public IEnumerable<Answer> Answers { get; private set; } = new List<Answer>();
        public IEnumerable<Comment> Comments { get; private set; } = new List<Comment>();

        private User()
        {
        }

        public void Edit(string username, string email, string websiteUrl, string aboutMe, ILimits limits)
        {
            Validate(username, email, websiteUrl, aboutMe, limits);
            Username = username;
            Email = new MailAddress(email);
            WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? null
                : new Uri(websiteUrl);
            AboutMe = aboutMe;
            CreatedOn = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }

        public static User Create(string username, string email, string websiteUrl, string aboutMe, ILimits limits)
        {
            Validate(username, email, websiteUrl, aboutMe, limits);
            User user = new User();
            user.Username = username;
            user.Email = new MailAddress(email);
            user.WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)
                ? null
                : new Uri(websiteUrl);
            user.AboutMe = aboutMe;
            user.CreatedOn = DateTime.UtcNow;
            user.LastSeen = DateTime.UtcNow;
            return user;
        }

        public void SeenNow() =>
            LastSeen = DateTime.UtcNow;

        private static void Validate(string username, string email, string websiteUrl, string aboutMe, ILimits limits)
        {
            if (username.Length < limits.UsernameMinimumLength || username.Length > limits.UsernameMaximumLength)
            {
                throw new BusinessException(
                    $"Username length must be between {limits.UsernameMinimumLength} and {limits.UsernameMaximumLength}.");
            }
            try
            {
                new MailAddress(email);
            }
            catch
            {
                throw new BusinessException("Email is not valid.");
            }
            if (!string.IsNullOrWhiteSpace(websiteUrl) && !Uri.TryCreate(websiteUrl, UriKind.Absolute, out var _))
            {
                throw new BusinessException("Website Url is not valid.");
            }
            if (aboutMe.Length > limits.AboutMeMaximumLength)
            {
                throw new BusinessException($"About Me is too long, must be at most {limits.AboutMeMaximumLength} characters.");
            }
        }
    }
}
