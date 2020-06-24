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
        public Guid OwnerId { get; private set; }
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

        private Vote(Guid ownerId, Question question, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            QuestionId = question.Id;
            VoteType = voteType;
            CreatedOn = DateTime.UtcNow;
        }

        private Vote(Guid ownerId, Answer answer, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            AnswerId = answer.Id;
            VoteType = voteType;
            CreatedOn = DateTime.UtcNow;
        }

        private Vote(Guid ownerId, Comment comment, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            CommentId = comment.Id;
            VoteType = voteType;
            CreatedOn = DateTime.UtcNow;
        }

        public static Vote CreateVote(Guid ownerId, Question question, VoteTypeEnum voteType) =>
            new Vote(ownerId, question, voteType);

        public static Vote CreateVote(Guid ownerId, Answer answer, VoteTypeEnum voteType) =>
            new Vote(ownerId, answer, voteType);

        public static Vote CreateVote(Guid ownerId, Comment comment, VoteTypeEnum voteType) =>
            new Vote(ownerId, comment, voteType);
    }
}