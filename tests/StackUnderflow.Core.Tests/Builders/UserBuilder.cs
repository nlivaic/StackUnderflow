using System;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Tests.Builders
{
    public class UserBuilder
    {
        private User _target;

        public UserBuilder BuildValidUser()
        {
            _target = User.Create(
                "normal_username",
                "normal.user.name@some_email.com",
                "http://normal_web_site.com/",
                "about_me",
                new LimitsBuilder().Build()
            );
            return this;
        }

        public UserBuilder BuildUser(Guid id)
        {
            _target = User.Create(
                "normal_username",
                "normal.user.name@some_email.com",
                "http://normal_web_site.com/",
                "about_me",
                new LimitsBuilder().Build()
            );
            _target.SetProperty(nameof(_target.Id), id);
            return this;
        }

        public UserBuilder MakeTimeGoBack()
        {
            _target.SetProperty(
                nameof(_target.CreatedOn),
                DateTime.UtcNow.AddMinutes(-10));
            _target.SetProperty(
                nameof(_target.LastSeen),
                DateTime.UtcNow.AddMinutes(-10));
            return this;
        }

        public User Build() =>
            _target;
    }
}