using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IQuestionRepository _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILimits _limits;
        private readonly IVoteable _voteable;
        private readonly ICommentable _commentable;

        public AnswerService(IUnitOfWork uow,
            IQuestionRepository questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<User> userRepository,
            IMapper mapper,
            ILimits limits)
        {
            _uow = uow;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _limits = limits;
            _voteable = new Voteable();
            _commentable = new Commentable();
        }

        public async Task<AnswerGetModel> PostAnswerAsync(AnswerCreateModel answerModel)
        {
            // @nl: check if null.
            // var owner = _ownerRepository.GetByIdAsync(ownerId);
            // if (owner == null)
            // {
            // }
            var question = (await _questionRepository.GetQuestionWithAnswersAsync(answerModel.QuestionId))
                ?? throw new BusinessException($"Question '{answerModel.QuestionId}' does not exist!");
            var user = await _userRepository.GetByIdAsync(answerModel.UserId);
            var answer = Answer.Create(user, answerModel.Body, question, _limits);
            question.Answer(answer);
            await _answerRepository.AddAsync(answer);
            await _uow.SaveAsync();
            return _mapper.Map<AnswerGetModel>(answer);
            // @nl: Raise an event! Message must be sent to the inbox.
        }

        public async Task EditAnswerAsync(AnswerEditModel answerModel)
        {
            var answer = (await
                _answerRepository
                    .GetSingleAsync(a =>
                        a.Id == answerModel.AnswerId
                        && a.UserId == answerModel.UserId
                        && a.QuestionId == answerModel.QuestionId))
                ?? throw new EntityNotFoundException(nameof(Answer), answerModel.AnswerId);
            var user = await _userRepository.GetByIdAsync(answerModel.UserId);
            answer.Edit(user, answerModel.Body, _limits);
            await _uow.SaveAsync();
        }

        public async Task<AnswerGetModel> AcceptAnswerAsync(AnswerAcceptModel answerModel)
        {
            // @nl: a da povučem (question+svi answeri)? Ili (question+taj specifičan answer)? Da idem iz repoa dva puta na bazu?
            var question = (await _questionRepository.GetByIdAsync(answerModel.QuestionId))
                ?? throw new EntityNotFoundException(nameof(Question), answerModel.QuestionId);
            var answer = await _answerRepository.GetSingleAsync(a => a.QuestionId == answerModel.QuestionId && a.Id == answerModel.AnswerId)
                ?? throw new EntityNotFoundException(nameof(Answer), answerModel.AnswerId);
            if (question.UserId != answerModel.QuestionUserId)
            {
                throw new BusinessException("Only question user can accept an answer!");
            }
            if (question.HasAcceptedAnswer)
            {
                throw new BusinessException($"Question already has an accepted answer!");
            }
            question.AcceptAnswer(answer);
            await _uow.SaveAsync();
            return _mapper.Map<AnswerGetModel>(answer);
            // @nl: calculate points.
            // @nl: raise an event. Message must be sent to answer owner's inbox.
        }

        public async Task DeleteAnswerAsync(Guid answerUserId, Guid questionId, Guid answerId)
        {
            var answer = (await
                _answerRepository
                    .GetSingleAsync(a =>
                        a.UserId == answerUserId
                        && a.Id == answerId
                        && a.QuestionId == questionId))
                ?? throw new EntityNotFoundException(nameof(Answer), answerId);
            if (answer.IsAcceptedAnswer)
            {
                throw new BusinessException($"Answer with id '{answerId}' has been accepted on '{answer.AcceptedOn}'.");
            }
            _answerRepository.Delete(answer);
            await _uow.SaveAsync();
        }
    }
}
