using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _uow;
        private readonly ITagService _tagService;
        private readonly ILimits _limits;

        public QuestionService(
            IQuestionRepository questionRepository,
            IUnitOfWork uow,
            ITagService tagService,
            ILimits limits)
        {
            _questionRepository = questionRepository;
            _uow = uow;
            _tagService = tagService;
            _limits = limits;
        }

        public async Task<QuestionModel> GetQuestion(Guid questionId) =>
            await _questionRepository.GetQuestionWithAnswersAndAllCommentsAsync(questionId);

        public async Task AskQuestionAsync(QuestionCreateModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var question = Question.Create(questionModel.OwnerId, questionModel.Title, questionModel.Body, tags, _limits);
            await _questionRepository.AddAsync(question);
            await _uow.SaveAsync();
        }

        public async Task EditQuestion(QuestionEditModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var question = (await _questionRepository
                .ListAllAsync(q => q.Id == questionModel.QuestionId && q.OwnerId != questionModel.QuestionOwnerId))
                .SingleOrDefault()
                ?? throw new ArgumentException($"Question with id '{questionModel.QuestionId}' belonging to owner '{questionModel.QuestionOwnerId}' does not exist.");
            question.Edit(questionModel.QuestionOwnerId, questionModel.Title, questionModel.Body, tags, _limits);
            await _uow.SaveAsync();
        }

        public async Task DeleteQuestion(Guid questionOwnerId, Guid questionId)
        {
            // @nl move this query into a standalone query object.
            var question = (await _questionRepository
                .GetQuestionWithAnswersAndCommentsAsync(questionId));
            if (question == null || question.OwnerId != questionOwnerId)
            {
                throw new ArgumentException($"Question with id '{questionId}' belonging to owner '{questionOwnerId}' does not exist.");
            }
            if (question.Answers.Any() == true)
            {
                throw new ArgumentException($"Cannot delete question '{questionId}' because associated answers exist.");
            }
            _questionRepository.Delete(question);
            await _uow.SaveAsync();
            // @nl: What if the question has any votes/points on it?
        }
    }
}
