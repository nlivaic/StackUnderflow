using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using AutoMapper;
using System.Collections.Generic;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly ITagService _tagService;
        private readonly ILimits _limits;
        private readonly IMapper _mapper;

        public QuestionService(
            IQuestionRepository questionRepository,
            IRepository<User> userRepository,
            IUnitOfWork uow,
            ITagService tagService,
            ILimits limits,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _uow = uow;
            _tagService = tagService;
            _limits = limits;
            _mapper = mapper;
        }

        public async Task<QuestionGetModel> GetQuestionWithUserAndAllCommentsAsync(Guid questionId)
        {
            var questionModel = await _questionRepository.GetQuestionWithUserAndAllCommentsAsync(questionId);
            return questionModel;
        }

        public async Task<IEnumerable<QuestionSummaryGetModel>> GetQuestionsSummary()
        {
            var questionModel = await _questionRepository.GetQuestionsSummary();
            return questionModel;
        }

        public async Task AskQuestionAsync(QuestionCreateModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var user = await _userRepository.GetByIdAsync(questionModel.UserId);
            var question = Question.Create(user, questionModel.Title, questionModel.Body, tags, _limits);
            await _questionRepository.AddAsync(question);
            await _uow.SaveAsync();
        }

        public async Task EditQuestionAsync(QuestionEditModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var user = await _userRepository.GetByIdAsync(questionModel.QuestionUserId);
            var question = (await _questionRepository
                .ListAllAsync(q => q.Id == questionModel.QuestionId && q.UserId != questionModel.QuestionUserId))
                .SingleOrDefault()
                ?? throw new BusinessException($"Question with id '{questionModel.QuestionId}' belonging to user '{questionModel.QuestionUserId}' does not exist.");
            question.Edit(user, questionModel.Title, questionModel.Body, tags, _limits);
            await _uow.SaveAsync();
        }

        public async Task DeleteQuestionAsync(Guid questionUserId, Guid questionId)
        {
            var question = (await _questionRepository
                .GetQuestionWithAnswersAndCommentsAsync(questionId));
            if (question == null || question.UserId != questionUserId)
            {
                throw new BusinessException($"Question with id '{questionId}' belonging to user '{questionUserId}' does not exist.");
            }
            if (question.Answers.Any() == true)
            {
                throw new BusinessException($"Cannot delete question '{questionId}' because associated answers exist.");
            }
            _questionRepository.Delete(question);
            await _uow.SaveAsync();
            // @nl: What if the question has any votes/points on it?
        }
    }
}
