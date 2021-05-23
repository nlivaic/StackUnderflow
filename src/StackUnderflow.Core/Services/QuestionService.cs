using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using AutoMapper;
using StackUnderflow.Infrastructure.Caching;
using StackUnderflow.Core.Models.Questions;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICommentService _commentService;
        private readonly ICache _cache;
        private readonly IVoteRepository _voteRepository;
        private readonly IVoteService _voteService;
        private readonly ITagService _tagService;
        private readonly BaseLimits _limits;
        private readonly IMapper _mapper;

        public QuestionService(
            IQuestionRepository questionRepository,
            IUserRepository userRepository,
            IUnitOfWork uow,
            ICommentService commentService,
            ICache cache,
            IVoteRepository voteRepository,
            IVoteService voteService,
            ITagService tagService,
            BaseLimits limits,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _uow = uow;
            _commentService = commentService;
            _cache = cache;
            _voteRepository = voteRepository;
            _voteService = voteService;
            _tagService = tagService;
            _limits = limits;
            _mapper = mapper;
        }

        public async Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(QuestionFindModel questionFindModel)
        {
            var question = await _questionRepository.GetQuestionWithUserAndTagsAsync(questionFindModel);
            question.VotesSum = await _voteService.GetVotesSumAsync(questionFindModel.Id, VoteTargetEnum.Question);
            return question;
        }

        public async Task<QuestionGetModel> AskQuestionAsync(QuestionCreateModel questionModel)
        {
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var user = await _userRepository.GetByIdAsync(questionModel.UserId);
            var question = Question.Create(user, questionModel.Title, questionModel.Body, tags, _limits);
            await _questionRepository.AddAsync(question);
            await _uow.SaveAsync();
            return _mapper.Map<QuestionGetModel>(question);
        }

        public async Task EditQuestionAsync(QuestionEditModel questionModel)
        {
            var question = (await _questionRepository
                .GetQuestionWithTagsAsync(questionModel.QuestionId))
                ?? throw new EntityNotFoundException(nameof(Question), questionModel.QuestionId);
            var tags = await _tagService.GetTagsAsync(questionModel.TagIds);
            var user = await _userRepository.GetUser<User>(questionModel.QuestionUserId);
            question.Edit(user, questionModel.Title, questionModel.Body, tags, _limits);
            await _uow.SaveAsync();
        }

        public async Task DeleteQuestionAsync(Guid questionId, Guid questionUserId)
        {
            var question = (await _questionRepository
                .GetQuestionWithAnswersAndCommentsAsync(questionId));
            if (question == null || question.UserId != questionUserId)
            {
                throw new EntityNotFoundException(nameof(Question), questionId);
            }
            if (question.Answers.Any() == true)
            {
                throw new BusinessException($"Cannot delete question '{questionId}' because associated answers exist.");
            }
            var votesSum = await _voteRepository.CountAsync(v => v.QuestionId == questionId);
            if (votesSum > 0)
            {
                throw new BusinessException($"Cannot delete question '{questionId}' because associated votes exist.");
            }
            await _commentService.DeleteRangeAsync(new CommentsDeleteModel
            {
                ParentQuestionId = questionId
            });
            _questionRepository.Delete(question);
            await _uow.SaveAsync();
        }
    }
}
