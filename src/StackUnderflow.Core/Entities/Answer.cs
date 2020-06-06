using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Entities
{
    public class Answer : BaseVoteable
    {
        public Guid OwnerId { get; private set; }
        public string Body { get; private set; }
        public bool IsAcceptedAnswer { get; private set; }
        public DateTime? AcceptedOn { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Question Question { get; private set; }
        public Guid QuestionId { get; private set; }
        public IEnumerable<Comment> Comments => _comments;

        private List<Comment> _comments;

        // public Answer(string body, Guid questionId, IEnumerable<Comment> comments)
        // {
        //     Body = body ?? throw new ArgumentException("Answer must have a body.");
        //     QuestionId = questionId == Guid.Empty
        //         ? throw new ArgumentException("Answer must relate to a question.")
        //         : questionId;
        //     _comments = comments == null || comments.Count() == 0
        //         ? new List<Comment>()
        //         : new List<Comment>(comments);
        //     CreatedOn = DateTime.UtcNow;
        // }

        public void AcceptedAnswer()
        {
            IsAcceptedAnswer = true;
            AcceptedOn = DateTime.UtcNow;
        }

        public void Edit(Guid ownerId, string body, ILimits limits)
        {
            if (OwnerId != ownerId)
            {
                throw new ArgumentException("Question can be edited only by owner.");
            }
            if (CreatedOn.Add(limits.AnswerEditDeadline) > DateTime.UtcNow)
            {
                throw new ArgumentException($"Answer with id '{Id}' cannot be edited since more than '{limits.AnswerEditDeadline.Minutes}' minutes passed.");
            }
            Body = body;
        }

        public static Answer Create(Guid ownerId, string body, Question question, ILimits limits)
        {
            var answer = new Answer();
            answer.Id = Guid.NewGuid();
            answer.OwnerId = ownerId;
            if (body.Length < limits.AnswerBodyMinimumLength)
            {
                throw new ArgumentException($"Answer body must be at least '{limits.AnswerBodyMinimumLength}' characters.");
            }
            answer.Body = body ?? throw new ArgumentException("Answer must have a body.");
            answer.IsAcceptedAnswer = false;
            answer.CreatedOn = DateTime.UtcNow;
            return answer;
        }
    }
}