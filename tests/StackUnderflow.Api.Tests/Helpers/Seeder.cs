using System;
using System.Collections.Generic;
using StackUnderflow.Core.Tests.Builders;
using StackUnderflow.Data;
using StackUnderflow.Data.Models;

namespace StackUnderflow.Api.Tests.Helpers
{
    public static class Seeder
    {
        public static void Seed(this StackUnderflowDbContext ctx)
        {
            var user = new UserBuilder().BuildUser(new Guid("00000000-0000-0000-0000-000000000001")).Build();
            var question = new QuestionBuilder().SetupValidQuestion(user).Build();
            var answer = new AnswerBuilder().SetupValidAnswer(user).Build();
            question.Answer(answer);
            var commentOnQuestion = new CommentBuilder().SetupValidComment(user).Build();
            var commentOnAnswer = new CommentBuilder().SetupValidComment(user).Build();
            question.Comment(commentOnQuestion);
            answer.Comment(commentOnAnswer);
            ctx.Questions.Add(question);
            ctx.LimitsKeyValuePairs.AddRange(
                new List<LimitsKeyValuePair>
                {
                    new LimitsKeyValuePair { LimitKey = "QuestionEditDeadline", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "AnswerEditDeadline", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "CommentEditDeadline", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "VoteEditDeadline", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "AcceptAnswerDeadline", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "QuestionBodyMinimumLength", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "AnswerBodyMinimumLength", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "CommentBodyMinimumLength", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "TagMinimumCount", LimitValue = 2 },
                    new LimitsKeyValuePair { LimitKey = "TagMaximumCount", LimitValue = 5 },
                    new LimitsKeyValuePair { LimitKey = "UsernameMinimumLength", LimitValue = 5 },
                    new LimitsKeyValuePair { LimitKey = "UsernameMaximumLength", LimitValue = 15 },
                    new LimitsKeyValuePair { LimitKey = "AboutMeMaximumLength", LimitValue = 10 },
                    new LimitsKeyValuePair { LimitKey = "UpvotePoints", LimitValue = 1 },
                    new LimitsKeyValuePair { LimitKey = "DownvotePoints", LimitValue = 1 }
                });
            ctx.SaveChanges();
        }
    }
}
