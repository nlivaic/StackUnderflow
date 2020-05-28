using System.Linq;
using StackUnderflow.Common.Repository;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepository;

        public QuestionService()
        {
        }

        public void AskQuestion(QuestionCreateModel question)
        {
            // @nl:
            // 1. Fetch tags based on tag id.
            // 2. Create a new question.
            // 3. Store the question.
        }

        public void EditQuestion(QuestionEditModel editedQuestion)
        {
            throw new System.NotImplementedException();
        }
    }
}