using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Interfaces
{
    interface IQuestionService
    {
        void AskQuestion(QuestionCreateModel question);
        void EditQuestion(QuestionEditModel editedQuestion);
    }

    public class QuestionEditModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Guid> Tags { get; set; }
    }

    public class QuestionCreateModel
    {
        public Guid OwnerId { get; set; }   // @nl
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}