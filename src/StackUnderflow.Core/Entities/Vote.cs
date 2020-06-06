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
        public DateTime CreatedAt { get; private set; }
        public Question Question { get; private set; }
        public Guid? QuestionId { get; private set; }
        public Answer Answer { get; private set; }
        public Guid? AnswerId { get; private set; }
        public Comment Comment { get; private set; }
        public Guid? CommentId { get; private set; }
        public VoteTypeEnum VoteType { get; private set; }

        private Vote(Guid ownerId, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            VoteType = voteType;
        }

        private Vote(Guid ownerId, Question question, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            QuestionId = question.Id;
            VoteType = voteType;
        }

        private Vote(Guid ownerId, Answer answer, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            AnswerId = answer.Id;
            VoteType = voteType;
        }

        private Vote(Guid ownerId, Comment comment, VoteTypeEnum voteType)
        {
            OwnerId = ownerId;
            CommentId = comment.Id;
            VoteType = voteType;
        }

        public static Vote CreateVote(Guid ownerId, VoteTypeEnum voteType) =>
            new Vote(ownerId, voteType);

        public static Vote CreateVoteOnQuestion(Guid ownerId, Question question, VoteTypeEnum voteType) =>
            new Vote(ownerId, question, voteType);

        public static Vote CreateVoteOnAnswer(Guid ownerId, Answer answer, VoteTypeEnum voteType) =>
            new Vote(ownerId, answer, voteType);

        public static Vote CreateVoteOnComment(Guid ownerId, Comment comment, VoteTypeEnum voteType) =>
            new Vote(ownerId, comment, voteType);
    }
}