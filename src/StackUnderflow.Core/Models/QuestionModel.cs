using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Models
{
    public class QuestionModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<AnswerModel> Answers { get; set; } = new List<AnswerModel>();
        public IEnumerable<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public IEnumerable<TagModel> Tags { get; set; } = new List<TagModel>();
    }
}