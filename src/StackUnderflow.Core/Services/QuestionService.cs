using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Query;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Specifications;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Question> _questionRepository;
        private readonly ITagService _tagService;

        public QuestionService(
            IUnitOfWork uow,
            IRepository<Question> questionRepository,
            ITagService tagService)
        {
            _uow = uow;
            _questionRepository = questionRepository;
            _tagService = tagService;
        }

        public async Task<Question> GetQuestion(Guid questionId)
        {
            var question = await _questionRepository
                .FirstOrDefaultAsync(new QuestionSpecification(questionId).IncludeAnswersAndComments());
            return question;
        }

        public async Task AskQuestionAsync(Guid ownerId, string title, string body, IEnumerable<Guid> tagIds)
        {
            var tags = await _tagService.GetTagsAsync(tagIds);
            var question = Question.Create(ownerId, title, body, tags);
            await _questionRepository.AddAsync(question);
            await _uow.SaveAsync();
        }

        public async Task EditQuestion(QuestionEditModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var question = await _questionRepository.GetByIdAsync(questionModel.QuestionId);
            if (question.OwnerId != questionModel.QuestionOwnerId)
            {
                throw new ArgumentException("Only question owner can delete the question.");

            }
            question.Edit(questionModel.Title, questionModel.Body, tags);
            await _uow.SaveAsync();
        }

        public async Task DeleteQuestion(Guid questionOwnerId, Guid questionId)
        {
            var question = await
                _questionRepository
                .FirstOrDefaultAsync(
                    new QuestionSpecification(questionId)
                        .IncludeQuestionCommentsAndAnswers());
            if (question.OwnerId != questionOwnerId)
            {
                throw new ArgumentException("Only question owner can delete the question.");
            }
            if (question?.Answers.Any() == true)
            {
                throw new ArgumentException($"Cannot delete question '{questionId}' because associated answers exist.");
            }
            if (question != null)
            {
                _questionRepository.Delete(question);

                await _uow.SaveAsync();
                // @nl: think about the points the owner got on this question!
            }
        }
    }
}
