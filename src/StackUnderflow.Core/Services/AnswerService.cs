using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Services
{
    public class AnswerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IDeadlineService _deadlineService;

        public AnswerService(IUnitOfWork uow, IRepository<Question> questionRepository, IRepository<Answer> answerRepository, IDeadlineService deadlineService)
        {
            _uow = uow;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _deadlineService = deadlineService;
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
            if ((await _answerRepository.ListAllAsync(a => a.QuestionId == questionId && a.OwnerId == ownerId)).SingleOrDefault() != null)
            {
                throw new ArgumentException($"User '{ownerId}' has already submitted an answer to question '{questionId.ToString()}'.");
            }
            var answer = Answer.Create(ownerId, body, question);
            await _answerRepository.AddAsync(answer);
            await _uow.SaveAsync();
            // @nl: Raise an event! Message must be sent to the inbox.
        }

        public async Task EditAnswer(Guid answerOwnerId, Guid answerId, string body)
        {
            var answer = (await _answerRepository.ListAllAsync(a => a.Id == answerId && a.OwnerId == answerOwnerId)).SingleOrDefault()
                ?? throw new ArgumentException($"Answer with id '{answerId}' belonging to owner '{answerOwnerId}' does not exist.");
            if (answer.CreatedOn.Add(_deadlineService.AnswerEditDeadline) > DateTime.UtcNow)
            {
                throw new ArgumentException($"Answer with id '{answerId}' cannot be edited since more than '{_deadlineService.AnswerEditDeadline.Minutes}' minutes passed.");
            }
            answer.Edit(body);
            await _uow.SaveAsync();

        }

        public async Task AcceptAnswer(Guid questionOwnerId, Guid questionId, Guid answerId)
        {
            var question = (await _questionRepository.GetByIdAsync(questionId))
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
            var answer = (await _answerRepository.ListAllAsync(a => a.OwnerId == answerOwnerId && a.Id == answerId)).SingleOrDefault()
                ?? throw new ArgumentException($"Answer with id '{answerId}' belonging to owner '{answerOwnerId}' does not exist.");
            if (answer.IsAcceptedAnswer)
            {
                throw new ArgumentException($"Answer with id '{answerId}' has been accepted on '{answer.AcceptedOn}'.");
            }
            _answerRepository.Delete(answer);
            await _uow.SaveAsync();
        }
    }
}
