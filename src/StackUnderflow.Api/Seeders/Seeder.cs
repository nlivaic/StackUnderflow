using System;
using System.Collections.Generic;
using StackUnderflow.Core.Entities;
using FizzWare.NBuilder;
using System.Linq;
using StackUnderflow.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Data;
using Microsoft.EntityFrameworkCore;

namespace StackUnderflow.Api.Seeders
{
    public static class Seeder
    {
        private static ILimits _limits = new Core.Services.Limits();
        private static List<Tag> _tags { get; set; }
        private static List<User> _users { get; set; }

        private static List<Question> ExecuteSeeder() =>
            BuildQuestions();

        private static void BuildTags() =>
            _tags = Builder<Tag>.CreateListOfSize(5).Build().ToList();

        private static void BuildUsers() =>
            _users = new List<User>
            {
                GenerateUser(),
                GenerateUser(),
                GenerateUser(),
                GenerateUser(),
                GenerateUser()
            };

        public static IHost Seed(this IHost host)
        {
            IHostEnvironment hostEnvironment = (IHostEnvironment)host.Services.GetService(typeof(IHostEnvironment));
            if (hostEnvironment.IsDevelopment())
            {
                using (var scope = host.Services.CreateScope())
                {
                    StackUnderflowDbContext context = (StackUnderflowDbContext)scope.ServiceProvider.GetService(typeof(StackUnderflowDbContext));
                    if (!context.Questions.Any())
                    {
                        BuildTags();
                        BuildUsers();
                        var questions = ExecuteSeeder();
                        context.Questions.AddRange(questions);
                        context.SaveChanges();
                    }
                }
            }
            return host;
        }

        private static List<Question> BuildQuestions()
        {
            var q1 = GenerateQuestion(_users[0], 3, _tags[0], _tags[3]);
            q1.Answer(GenerateAnswer(_users[2], q1));
            q1.AcceptAnswer(q1.Answers.ToList()[0]);

            var q2 = GenerateQuestion(_users[1], 3, _tags[1], _tags[2]);

            var q3 = GenerateQuestion(_users[2], 3, _tags[0], _tags[1], _tags[2], _tags[3]);
            q3.Answer(GenerateAnswer(_users[2], q3));
            q3.Answer(GenerateAnswer(_users[3], q3));
            q3.Answer(GenerateAnswer(_users[4], q3));

            var q4 = GenerateQuestion(_users[3], 5, _tags[0], _tags[3], _tags[4]);

            var q5 = GenerateQuestion(_users[1], 0, _tags[0], _tags[1], _tags[3]);
            q5.Answer(GenerateAnswer(_users[0], q5, 2));
            q5.Answer(GenerateAnswer(_users[1], q5, 3));
            q5.Answer(GenerateAnswer(_users[4], q5, 10));
            q5.AcceptAnswer(q5.Answers.ToList()[2]);

            var q6 = GenerateQuestion(_users[1], 1, _tags[0], _tags[2], _tags[3]);
            q6.Answer(GenerateAnswer(_users[0], q6, 4));
            q6.Answer(GenerateAnswer(_users[1], q6, 5));
            q6.Answer(GenerateAnswer(_users[4], q6, 6));
            q6.AcceptAnswer(q6.Answers.ToList()[1]);

            var q7 = GenerateQuestion(_users[1], 12, _tags[2], _tags[3], _tags[1]);
            q7.Answer(GenerateAnswer(_users[0], q7));
            q7.Answer(GenerateAnswer(_users[1], q7));
            q7.Answer(GenerateAnswer(_users[4], q7));
            q7.AcceptAnswer(q7.Answers.ToList()[0]);

            var questions = new List<Question> { q1, q2, q3, q4, q5, q6, q7 };
            return questions;
        }

        private static string GenerateBody() => string.Join(" ", Faker.Lorem.Sentences(5));

        private static Question GenerateQuestion(User user, int commentCount, params Tag[] tags)
        {
            var question = Question.Create(
                user,
                Faker.Lorem.Sentence(),
                GenerateBody(),
                new List<Tag>(tags),
                _limits);
            for (int i = 0; i < commentCount; i++)
            {
                question.Comment(GenerateComment(_users[new Random().Next(5)], i + 1));
            }
            return question;
        }

        private static Comment GenerateComment(User user, int orderNumber) =>
            Comment.Create(
                user,
                GenerateBody(),
                orderNumber,
                _limits);

        private static Answer GenerateAnswer(User user, Question question, int commentCount = 0)
        {
            var answer = Answer.Create(
                user,
                GenerateBody(),
                question,
                _limits);
            for (int i = 0; i < commentCount; i++)
            {
                answer.Comment(GenerateComment(_users[new Random().Next(5)], i + 1));
            }
            return answer;
        }

        private static User GenerateUser() =>
            User.Create(
                _limits,
                Guid.NewGuid(),
                Faker.Name.Last().PadRight(10, 'a').Substring(0, new Random().Next(7, 10)),
                Faker.Internet.Email(),
                Faker.Internet.Url(),
                Faker.Lorem.Paragraph(4).PadRight(200, 'a').Substring(0, new Random().Next(0, 200)));

        private static Tag ById(this DbSet<Tag> tags, int id) =>
            tags.First(t => t.Id == new Guid($"00000000-0000-0000-0000-00000000000{id}"));

        private static User ByIndex(this DbSet<User> users, int index) =>
            users.ToList()[index];
    }
}
