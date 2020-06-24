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

        private static List<Guid> Owners =>
            new List<Guid>
            {
                new Guid("00000000-0000-0000-0000-000000000001"),
                new Guid("00000000-0000-0000-0000-000000000002"),
                new Guid("00000000-0000-0000-0000-000000000003"),
                new Guid("00000000-0000-0000-0000-000000000004"),
                new Guid("00000000-0000-0000-0000-000000000005")
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
            var q1 = GenerateQuestion(Owners[0], tags.ById(1), tags.ById(4));
            q1.Comment(GenerateComent(Owners[0], 1));
            q1.Comment(GenerateComent(Owners[1], 2));
            q1.Comment(GenerateComent(Owners[3], 3));
            q1.Answer(GenerateAnswer(Owners[2], q1));
            q1.AcceptAnswer(q1.Answers.ToList()[0]);

            var q2 = GenerateQuestion(Owners[1], tags.ById(2), tags.ById(3));
            q2.Comment(GenerateComent(Owners[1], 1));
            q2.Comment(GenerateComent(Owners[2], 2));
            q2.Comment(GenerateComent(Owners[1], 3));

            var q3 = GenerateQuestion(Owners[2], tags.ById(1), tags.ById(2), tags.ById(3), tags.ById(4));
            // q3.Comment(GenerateComent(Owners[2], 1));
            // q3.Comment(GenerateComent(Owners[1], 2));
            // q3.Comment(GenerateComent(Owners[4], 3));
            // q3.Answer(GenerateAnswer(Owners[2], q3));
            // q3.Answer(GenerateAnswer(Owners[3], q3));
            // q3.Answer(GenerateAnswer(Owners[4], q3));

            // var q4 = GenerateQuestion(Owners[3], _tags[0], _tags[3], _tags[4]);
            // q4.Comment(GenerateComent(Owners[2], 1));
            // q4.Comment(GenerateComent(Owners[1], 2));
            // q4.Comment(GenerateComent(Owners[4], 3));
            // q4.Comment(GenerateComent(Owners[4], 4));
            // q4.Comment(GenerateComent(Owners[4], 5));

            // var q5 = GenerateQuestion(Owners[1], _tags[0], _tags[1], _tags[3]);
            // q5.Answer(GenerateAnswer(Owners[0], q5));
            // q5.Answer(GenerateAnswer(Owners[1], q5));
            // q5.Answer(GenerateAnswer(Owners[4], q5));
            // q5.AcceptAnswer(q5.Answers.ToList()[2]);

            var questions = new List<Question> { q1, q2, q3/*, q4, q5*/ };
            return questions;
        }

        private static string GenerateBody() => string.Join(" ", Faker.Lorem.Sentences(5));

        private static Question GenerateQuestion(Guid ownerId, params Tag[] tags) =>
            Question.Create(
                ownerId,
                Faker.Lorem.Sentence(),
                GenerateBody(),
                new List<Tag>(tags),
                _limits,
                new Voteable(),
                new Commentable());

        private static Comment GenerateComent(Guid ownerId, int orderNumber) =>
            Comment.Create(
                ownerId,
                GenerateBody(),
                orderNumber,
                _limits,
                 new Voteable()
            );

        private static Answer GenerateAnswer(Guid ownerId, Question question) =>
            Answer.Create(
                ownerId,
                GenerateBody(),
                question,
                _limits,
                 new Voteable(),
                new Commentable()
            );

        private static Tag ById(this DbSet<Tag> tags, int id) =>
            tags.First(t => t.Id == new Guid($"00000000-0000-0000-0000-00000000000{id}"));
    }
}
