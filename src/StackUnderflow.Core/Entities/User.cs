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
        public DateTime CreatedAt { get; private set; }
        public DateTime LastSeen { get; private set; }
        public IEnumerable<Question> Questions { get; private set; }
        public IEnumerable<Answer> Answers { get; private set; }
        public IEnumerable<Comment> Comments { get; private set; }

        private User()
        {
        }

        public void Edit(string username, string email, string websiteUrl, string aboutMe, ILimits limits)
        {
            Validate(username, email, websiteUrl, aboutMe, limits);
            Username = username;
            Email = new MailAddress(email);
            WebsiteUrl = new Uri(websiteUrl);
            AboutMe = aboutMe;
            CreatedAt = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }

        public static User Create(string username, string email, string websiteUrl, string aboutMe, ILimits limits)
        {
            Validate(username, email, websiteUrl, aboutMe, limits);
            User user = new User();
            user.Username = username;
            user.Email = new MailAddress(email);
            user.WebsiteUrl = new Uri(websiteUrl);
            user.AboutMe = aboutMe;
            user.CreatedAt = DateTime.UtcNow;
            user.LastSeen = DateTime.UtcNow;
            return user;
        }

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
            if (!Uri.TryCreate(websiteUrl, UriKind.Absolute, out var _))
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
