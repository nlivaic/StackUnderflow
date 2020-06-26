using System;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Tests.Builders;
using Xunit;

namespace StackUnderflow.Core.Tests
{
    public class UserTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("http://normal_web_site.com/", "")]
        [InlineData("https://normal_web_site.com/", "")]
        [InlineData("https://normal_web_site.com/", "about_me")]
        public void User_CanCreateWithValidData_Successfully(string websiteUrl, string aboutMe)
        {
            // Arrange
            string username = "normal_username";
            string email = "normal.user.name@some_email.com";
            var limits = new LimitsBuilder().Build();

            // Act
            var result = User.Create(username, email, websiteUrl, aboutMe, limits);

            // Assert
            var resultWebsiteUrl = result.WebsiteUrl?.ToString();
            Assert.NotNull(result);
            Assert.Equal(result.Username, username);
            Assert.Equal(result.Email.Address, email);
            Assert.Equal(resultWebsiteUrl, string.IsNullOrWhiteSpace(websiteUrl) ? null : websiteUrl);
            Assert.Equal(result.AboutMe, aboutMe);
            Assert.True(DateTime.UtcNow - result.CreatedOn < TimeSpan.FromSeconds(1));
            Assert.True(DateTime.UtcNow - result.LastSeen < TimeSpan.FromSeconds(1));
            Assert.Empty(result.Questions);
            Assert.Empty(result.Answers);
            Assert.Empty(result.Comments);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("a", "normal.user.name@some_email.com", "http://normal_web_site.com/", "")]
        [InlineData("normal", "invalid.user.name@", "http://normal_web_site.com/", "")]
        [InlineData("very_long_username", "normal.user.name@some_email.com", "http://normal_web_site.com/", "")]
        [InlineData("normal", "normal.user.name@some_email.com", "invalid_web_site/", "")]
        [InlineData("normal", "normal.user.name@some_email.com", "http://normal_web_site/", "very_very_very_very_long_about_me")]
        public void User_CreatingWithInvalidData_Throws(string username, string email, string websiteUrl, string aboutMe)
        {
            // Arrange
            var limits = new LimitsBuilder().Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() => User.Create(username, email, websiteUrl, aboutMe, limits));
        }

        [Theory]
        [InlineData("newUsername", "new.normal.user.name@some_email.com", "http://new_normal_web_site.com/", "newAboutMe")]
        [InlineData("newUsername", "new.normal.user.name@some_email.com", "", "")]
        public void User_CanEditWithValidData_Successfully(string username, string email, string websiteUrl, string aboutMe)
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var target = new UserBuilder().BuildValidUser().Build();

            // Act
            target.Edit(username, email, websiteUrl, aboutMe, limits);

            // Assert
            var resultWebsiteUrl = target.WebsiteUrl?.ToString();
            Assert.Equal(target.Username, username);
            Assert.Equal(target.Email.Address, email);
            Assert.Equal(resultWebsiteUrl, string.IsNullOrWhiteSpace(websiteUrl) ? null : websiteUrl);
            Assert.Equal(target.AboutMe, aboutMe);
            Assert.True(DateTime.UtcNow - target.CreatedOn < TimeSpan.FromSeconds(1));
            Assert.Empty(target.Questions);
            Assert.Empty(target.Answers);
            Assert.Empty(target.Comments);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("a", "normal.user.name@some_email.com", "http://normal_web_site.com/", "")]
        [InlineData("normal", "invalid.user.name@", "http://normal_web_site.com/", "")]
        [InlineData("very_long_username", "normal.user.name@some_email.com", "http://normal_web_site.com/", "")]
        [InlineData("normal", "normal.user.name@some_email.com", "invalid_web_site/", "")]
        [InlineData("normal", "normal.user.name@some_email.com", "http://normal_web_site/", "very_very_very_very_long_about_me")]
        public void User_EditingWithInvalidData_Throws(string username, string email, string websiteUrl, string aboutMe)
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var target = new UserBuilder().BuildValidUser().Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.Edit(username, email, websiteUrl, aboutMe, limits));
        }

        [Fact]
        public void User_SeenNow_Successfully()
        {
            // Arrange
            var target = new UserBuilder()
                .BuildValidUser()
                .MakeTimeGoBack()
                .Build();

            // Act
            target.SeenNow();

            // Assert
            Assert.True(DateTime.UtcNow - target.LastSeen < TimeSpan.FromSeconds(1));
        }

    }
}