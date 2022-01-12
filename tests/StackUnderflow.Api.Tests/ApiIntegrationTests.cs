using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StackUnderflow.Api.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace StackUnderflow.Api.Tests
{
    public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ApiIntegrationTests(
            ITestOutputHelper testOutput,
            CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/");
            _testOutput = testOutput;
            _factory = factory;
        }

        [Fact]
        public async Task Api_CreateNewQuestion_SuccessfullyWith201()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var ctx = new SQLiteDbBuilder(_testOutput, _factory.KeepAliveConnection).BuildContext();
            var initialCount = ctx.Questions.Count();
            string[] tagIds = new string[] { "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002" };

            // Act
            var response = await client.PostAsJsonAsync("questions", new
            {
                Title = "My_Test_Title",
                Body = "My_Test_Body",
                TagIds = tagIds
            });
            using var ctx1 = new SQLiteDbBuilder(_testOutput, _factory.KeepAliveConnection).BuildContext();

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(initialCount + 1, ctx1.Questions.Count());
        }

        [Fact]
        public async Task Api_TwoAnswersFromSameUser_FailWith422()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            using var ctx = new SQLiteDbBuilder(_testOutput, _factory.KeepAliveConnection).BuildContext();
            var questionId = ctx.Questions.Where(q => q.Answers.Count() == 1).First().Id;
            var response = await client.PostAsJsonAsync($"questions/{questionId}/answers", new
            {
                Body = "My_Test_Answer_Body"
            });

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }
    }
}
