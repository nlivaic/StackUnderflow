using System;
using StackUnderflow.Common.Base;

namespace StackUnderflow.Core.Entities
{
    public class Vote : BaseEntity<Guid>
    {
        public enum VoteTypeEnum
        {
            Upvote = 1,
            Downvote = 2
        }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Question Question { get; private set; }
        public Guid? QuestionId { get; private set; }
        public Answer Answer { get; private set; }
        public Guid? AnswerId { get; private set; }
        public Comment Comment { get; private set; }
        public Guid? CommentId { get; private set; }
        public VoteTypeEnum VoteType { get; private set; }

        private Vote()
        {
        }

        private Vote(Guid userId, Question question, VoteTypeEnum voteType)
        {
            UserId = userId;
            QuestionId = question.Id;
            VoteType = voteType;
            CreatedOn = DateTime.UtcNow;
        }

        private Vote(Guid userId, Answer answer, VoteTypeEnum voteType)
        {
            UserId = userId;
            AnswerId = answer.Id;
            VoteType = voteType;
            CreatedOn = DateTime.UtcNow;
        }

        private Vote(Guid userId, Comment comment, VoteTypeEnum voteType)
        {
            UserId = userId;
            CommentId = comment.Id;
            VoteType = voteType;
            CreatedOn = DateTime.UtcNow;
        }

        public static Vote CreateVote(Guid userId, Question question, VoteTypeEnum voteType) =>
            new Vote(userId, question, voteType);

        public static Vote CreateVote(Guid userId, Answer answer, VoteTypeEnum voteType) =>
            new Vote(userId, answer, voteType);

        public static Vote CreateVote(Guid userId, Comment comment, VoteTypeEnum voteType) =>
            new Vote(userId, comment, voteType);
    }
}