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
        private static List<Tag> _tags => Builder<Tag>.CreateListOfSize(5).Build().ToList();
        private static ILimits _limits = new Core.Services.Limits();

        private static List<User> Users =>
            new List<User>
            {
                GenerateUser(),
                GenerateUser(),
                GenerateUser(),
                GenerateUser(),
                GenerateUser()
            };

        private static List<Question> ExecuteSeeder(DbSet<Tag> tags) =>
            BuildQuestions(tags);

        public static IHost Seed(this IHost host)
        {
            IHostEnvironment hostEnvironment = (IHostEnvironment)host.Services.GetService(typeof(IHostEnvironment));
            if (hostEnvironment.IsDevelopment())
            {
                using (var scope = host.Services.CreateScope())
                {
                    StackUnderflowDbContext context = (StackUnderflowDbContext)scope.ServiceProvider.GetService(typeof(StackUnderflowDbContext));
                    context.Tags.AddRange(_tags);
                    context.SaveChanges();
                    var questions = ExecuteSeeder(context.Tags);
                    context.Questions.AddRange(questions[0]);
                    context.SaveChanges();
                    context.Questions.AddRange(questions[1]);
                    context.SaveChanges();
                    context.Questions.AddRange(questions[2]);
                    context.SaveChanges();
                }
            }
            return host;
        }

        private static List<Question> BuildQuestions(DbSet<Tag> tags)
        {
            var q1 = GenerateQuestion(Users[0].Id, tags.ById(1), tags.ById(4));
            q1.Comment(GenerateComent(Users[0].Id, 1));
            q1.Comment(GenerateComent(Users[1].Id, 2));
            q1.Comment(GenerateComent(Users[3].Id, 3));
            q1.Answer(GenerateAnswer(Users[2].Id, q1));
            q1.AcceptAnswer(q1.Answers.ToList()[0]);

            var q2 = GenerateQuestion(Users[1].Id, tags.ById(2), tags.ById(3));
            q2.Comment(GenerateComent(Users[1].Id, 1));
            q2.Comment(GenerateComent(Users[2].Id, 2));
            q2.Comment(GenerateComent(Users[1].Id, 3));

            var q3 = GenerateQuestion(Users[2].Id, tags.ById(1), tags.ById(2), tags.ById(3), tags.ById(4));
            q3.Comment(GenerateComent(Users[2].Id, 1));
            q3.Comment(GenerateComent(Users[1].Id, 2));
            q3.Comment(GenerateComent(Users[4].Id, 3));
            q3.Answer(GenerateAnswer(Users[2].Id, q3));
            q3.Answer(GenerateAnswer(Users[3].Id, q3));
            q3.Answer(GenerateAnswer(Users[4].Id, q3));

            var q4 = GenerateQuestion(Users[3].Id, _tags[0], _tags[3], _tags[4]);
            q4.Comment(GenerateComent(Users[2].Id, 1));
            q4.Comment(GenerateComent(Users[1].Id, 2));
            q4.Comment(GenerateComent(Users[4].Id, 3));
            q4.Comment(GenerateComent(Users[4].Id, 4));
            q4.Comment(GenerateComent(Users[4].Id, 5));

            var q5 = GenerateQuestion(Users[1].Id, _tags[0], _tags[1], _tags[3]);
            q5.Answer(GenerateAnswer(Users[0].Id, q5));
            q5.Answer(GenerateAnswer(Users[1].Id, q5));
            q5.Answer(GenerateAnswer(Users[4].Id, q5));
            q5.AcceptAnswer(q5.Answers.ToList()[2]);

            var questions = new List<Question> { q1, q2, q3/*, q4, q5*/ };
            return questions;
        }

        private static string GenerateBody() => string.Join(" ", Faker.Lorem.Sentences(5));

        private static Question GenerateQuestion(Guid userId, params Tag[] tags) =>
            Question.Create(
                userId,
                Faker.Lorem.Sentence(),
                GenerateBody(),
                new List<Tag>(tags),
                _limits,
                new Voteable(),
                new Commentable());

        private static Comment GenerateComent(Guid userId, int orderNumber) =>
            Comment.Create(
                userId,
                GenerateBody(),
                orderNumber,
                _limits,
                 new Voteable()
            );

        private static Answer GenerateAnswer(Guid userId, Question question) =>
            Answer.Create(
                userId,
                GenerateBody(),
                question,
                _limits,
                 new Voteable(),
                new Commentable()
            );

        private static User GenerateUser() =>
            User.Create(
                Faker.Name.Last().PadRight(10, 'a').Substring(0, new Random().Next(7, 10)),
                Faker.Internet.Email(),
                Faker.Internet.Url(),
                Faker.Lorem.Paragraph(4).PadRight(200, 'a').Substring(0, new Random().Next(0, 200)),
                _limits);

        private static Tag ById(this DbSet<Tag> tags, int id) =>
            tags.First(t => t.Id == new Guid($"00000000-0000-0000-0000-00000000000{id}"));
    }
}