using System.Linq;
using AutoMapper;
using Xunit;

namespace StackUnderflow.Application.Tests
{
    public class AutoMapperConfigurationsTests
    {
        [Fact]
        public void AutoMapperConfigurations_AreAllValid()
        {
            // Arrange - get all profiles in the StackUnderflow.Application assembly.
            var profiles = AssemblyInfo.Value.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t));

            // Act
            var target = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
                //cfg.AddProfile(typeof(QuestionSummaryGetModelProfile));
                //cfg.AddProfile(typeof(TagProfile));
            }).CreateMapper();

            // Assert
            target.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
