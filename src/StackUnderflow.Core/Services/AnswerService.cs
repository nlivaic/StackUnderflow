using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Query;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Specifications;

namespace StackUnderflow.Core.Services
{
    public class AnswerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;

        public AnswerService(IUnitOfWork uow, IRepository<Question> questionRepository, IRepository<Answer> answerRepository)
        {
            _uow = uow;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public async Task PostAnswer(Guid questionId, Guid ownerId, string body)
        {
            // @nl: check if null.
            // var owner = _ownerRepository.GetByIdAsync(ownerId);
            // if (owner == null)
            // {
            // }
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new ArgumentException($"Question '{questionId}' does not exist!");
            }
            if ((await _answerRepository.FirstAsync(a => a.QuestionId == questionId && a.OwnerId == ownerId)) != null)
            {
                throw new ArgumentException($"User '{ownerId}' has already submitted an answer to question '{questionId.ToString()}'.");
            }
            var answer = Answer.Create(ownerId, body, question);
            await _answerRepository.AddAsync(answer);
            await _uow.SaveAsync();
            // @nl: Raise an event! Message must be sent to the inbox.
        }

        public async Task AcceptAnswer(Guid questionOwnerId, Guid questionId, Guid answerId)
        {
            var question = (await _questionRepository.FirstOrDefaultAsync(new QuestionSpecification(questionId)))
                ?? throw new ArgumentException($"Question '{questionId}' does not exist!");
            var answer = question.Answers.SingleOrDefault(a => a.Id == answerId)
                ?? throw new ArgumentException($"Answer '{answerId}' is not associated with question '{questionId}'!");
            if (question.OwnerId != questionOwnerId)
            {
                throw new ArgumentException("Only question owner can accept an answer!");
            }
            if (question.HasAcceptedAnswer)
            {
                throw new ArgumentException($"Question already has an accepted answer!");
            }
            question.AcceptAnswer();
            answer.AcceptedAnswer();
            await _uow.SaveAsync();
            // @nl: calculate points.
            // @nl: raise an event. Message must be sent to answer owner's inbox.
        }

        public async Task DeleteAnswer(Guid answerOwnerId, Guid answerId)
        {

        }

    }
}
