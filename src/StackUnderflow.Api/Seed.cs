using System;
using System.Collections.Generic;
using StackUnderflow.Core.Entities;
using FizzWare.NBuilder;
using System.Linq;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api
{
    public static class Seed
    {
        public static List<Tag> _tags => Builder<Tag>.CreateListOfSize(5).Build().ToList();
        public static ILimits _limits = new Core.Services.Limits();

        public static List<Guid> Owners =>
            new List<Guid>
            {
                new Guid("00000000-0000-0000-0000-000000000001"),
                new Guid("00000000-0000-0000-0000-000000000002"),
                new Guid("00000000-0000-0000-0000-000000000003"),
                new Guid("00000000-0000-0000-0000-000000000004"),
                new Guid("00000000-0000-0000-0000-000000000005")
            };

        public static List<Question> Execute() =>
            Questions();

        public static List<Question> Questions()
        {
            var q1 = GenerateQuestion(Owners[0], _tags[0], _tags[4]);
            q1.Comment(GenerateComent(Owners[0], 1));
            q1.Comment(GenerateComent(Owners[1], 2));
            q1.Comment(GenerateComent(Owners[3], 3));
            q1.Answer(GenerateAnswer(Owners[2], q1));
            q1.AcceptAnswer(q1.Answers.ToList()[0]);

            var q2 = GenerateQuestion(Owners[1], _tags[2], _tags[3]);
            q2.Comment(GenerateComent(Owners[1], 1));
            q2.Comment(GenerateComent(Owners[2], 2));
            q2.Comment(GenerateComent(Owners[1], 3));

            var q3 = GenerateQuestion(Owners[2], _tags[0], _tags[1], _tags[3], _tags[4]);
            q3.Comment(GenerateComent(Owners[2], 1));
            q3.Comment(GenerateComent(Owners[1], 2));
            q3.Comment(GenerateComent(Owners[4], 3));
            q3.Answer(GenerateAnswer(Owners[2], q3));
            q3.Answer(GenerateAnswer(Owners[3], q3));
            q3.Answer(GenerateAnswer(Owners[4], q3));

            var q4 = GenerateQuestion(Owners[3], _tags[0], _tags[3], _tags[4]);
            q4.Comment(GenerateComent(Owners[2], 1));
            q4.Comment(GenerateComent(Owners[1], 2));
            q4.Comment(GenerateComent(Owners[4], 3));
            q4.Comment(GenerateComent(Owners[4], 4));
            q4.Comment(GenerateComent(Owners[4], 5));

            var q5 = GenerateQuestion(Owners[1], _tags[0], _tags[1], _tags[3]);
            q5.Answer(GenerateAnswer(Owners[0], q5));
            q5.Answer(GenerateAnswer(Owners[1], q5));
            q5.Answer(GenerateAnswer(Owners[4], q5));
            q5.AcceptAnswer(q5.Answers.ToList()[2]);

            var questions = new List<Question> { q1, q2, q3, q4, q5 };
            return questions;
        }

        public static string GenerateBody() => string.Join(" ", Faker.Lorem.Sentences(5));

        public static Question GenerateQuestion(Guid ownerId, params Tag[] tags) =>
            Question.Create(
                ownerId,
                Faker.Lorem.Sentence(),
                GenerateBody(),
                new List<Tag>(tags),
                _limits,
                new Voteable(),
                new Commentable());

        public static Comment GenerateComent(Guid ownerId, int orderNumber) =>
            Comment.Create(
                ownerId,
                GenerateBody(),
                orderNumber,
                _limits,
                 new Voteable()
            );

        public static Answer GenerateAnswer(Guid ownerId, Question question) =>
            Answer.Create(
                ownerId,
                GenerateBody(),
                question,
                _limits,
                 new Voteable(),
                new Commentable()
            );
    }
}
