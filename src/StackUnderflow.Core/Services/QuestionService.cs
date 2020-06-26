using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using AutoMapper;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _uow;
        private readonly ITagService _tagService;
        private readonly ILimits _limits;
        private readonly IVoteable _voteable;
        private readonly ICommentable _commentable;
        private readonly IMapper mapper;

        public QuestionService(
            IQuestionRepository questionRepository,
            IUnitOfWork uow,
            ITagService tagService,
            ILimits limits,
            IVoteable voteable,
            ICommentable commentable,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _uow = uow;
            _tagService = tagService;
            _limits = limits;
            _voteable = voteable;
            _commentable = commentable;
            this.mapper = mapper;
        }

        public async Task<QuestionModel> GetQuestion(Guid questionId)
        {
            var q = await _questionRepository.GetQuestionWithUserAndAllCommentsAsync(questionId);
            return mapper.Map<QuestionModel>(q);

        }

        public async Task AskQuestionAsync(QuestionCreateModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var question = Question.Create(questionModel.UserId, questionModel.Title, questionModel.Body, tags, _limits, _voteable, _commentable);
            await _questionRepository.AddAsync(question);
            await _uow.SaveAsync();
        }

        public async Task EditQuestion(QuestionEditModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var question = (await _questionRepository
                .ListAllAsync(q => q.Id == questionModel.QuestionId && q.UserId != questionModel.QuestionUserId))
                .SingleOrDefault()
                ?? throw new BusinessException($"Question with id '{questionModel.QuestionId}' belonging to user '{questionModel.QuestionUserId}' does not exist.");
            question.Edit(questionModel.QuestionUserId, questionModel.Title, questionModel.Body, tags, _limits);
            await _uow.SaveAsync();
        }

        public async Task DeleteQuestion(Guid questionUserId, Guid questionId)
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
