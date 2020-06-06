using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Services
{
    public class AnswerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IQuestionRepository _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly ILimits _limits;

        public AnswerService(IUnitOfWork uow,
            IQuestionRepository questionRepository,
            IRepository<Answer> answerRepository,
            ILimits limits)
        {
            _uow = uow;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _limits = limits;
        }

        public async Task PostAnswer(AnswerCreateModel answerModel)
        {
            // @nl: check if null.
            // var owner = _ownerRepository.GetByIdAsync(ownerId);
            // if (owner == null)
            // {
            // }
            var question = (await _questionRepository.GetQuestionWithAnswersAsync(answerModel.QuestionId))
                ?? throw new ArgumentException($"Question '{answerModel.QuestionId}' does not exist!");
            var answer = Answer.Create(answerModel.OwnerId, answerModel.Body, question, _limits);
            question.Answer(answer);
            await _uow.SaveAsync();
            // @nl: Raise an event! Message must be sent to the inbox.
        }

        public async Task EditAnswer(AnswerEditModel answerModel)
        {
            var answer = (await _answerRepository.ListAllAsync(a => a.Id == answerModel.AnswerId && a.OwnerId == answerModel.OwnerId)).SingleOrDefault()
                ?? throw new ArgumentException($"Answer with id '{answerModel.AnswerId}' belonging to owner '{answerModel.OwnerId}' does not exist.");
            answer.Edit(answerModel.OwnerId, answerModel.Body, _limits);
            await _uow.SaveAsync();
        }

        public async Task AcceptAnswer(AnswerAcceptModel answerModel)
        {
            var question = (await _questionRepository.GetByIdAsync(answerModel.QuestionId))
                ?? throw new ArgumentException($"Question '{answerModel.QuestionId}' does not exist!");
            var answer = question.Answers.SingleOrDefault(a => a.Id == answerModel.AnswerId)
                ?? throw new ArgumentException($"Answer '{answerModel.AnswerId}' is not associated with question '{answerModel.QuestionId}'!");
            if (question.OwnerId != answerModel.QuestionOwnerId)
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