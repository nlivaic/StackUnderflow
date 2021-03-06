using System;
using System.Collections.Generic;
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
    public class CommentService : ICommentService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly ILimits _limits;
        private readonly IVoteable _voteable;
        private readonly IMapper _mapper;

        public CommentService(IQuestionRepository questionRepository,
            ICommentRepository commentRepository,
            IAnswerRepository answerRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILimits limits,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _uow = unitOfWork;
            _limits = limits;
            _voteable = new Voteable();
            _mapper = mapper;
        }

        public async Task<CommentForQuestionGetModel> CommentOnQuestionAsync(CommentOnQuestionCreateModel commentModel)
        {
            var question = (await _questionRepository.GetQuestionWithCommentsAsync(commentModel.QuestionId))
                ?? throw new EntityNotFoundException(nameof(Question), commentModel.QuestionId);
            var user = await _userRepository.GetByIdAsync(commentModel.UserId);
            var commentOrderNumber = question
                .Comments
                .Select(c => c.OrderNumber)
                .OrderByDescending(c => c)
                .FirstOrDefault() + 1;
            var comment = Comment.Create(user, commentModel.Body, commentOrderNumber, _limits);
            question.Comment(comment);
            await _commentRepository.AddAsync(comment);
            await _uow.SaveAsync();
            return _mapper.Map<CommentForQuestionGetModel>(comment);
        }

        public async Task<CommentForAnswerGetModel> CommentOnAnswerAsync(CommentOnAnswerCreateModel commentModel)
        {
            var user = await _userRepository.GetByIdAsync(commentModel.UserId);
            var answer = await _answerRepository.GetAnswerWithCommentsAsync(commentModel.QuestionId, commentModel.AnswerId);
            if (answer == null)
            {
                throw new EntityNotFoundException(nameof(Answer), commentModel.AnswerId);
            }
            var commentOrderNumber = answer
                .Comments
                .Select(c => c.OrderNumber)
                .OrderByDescending(c => c)
                .FirstOrDefault() + 1;
            var comment = Comment.Create(user, commentModel.Body, commentOrderNumber, _limits);
            answer.Comment(comment);
            await _commentRepository.AddAsync(comment);
            await _uow.SaveAsync();
            return _mapper.Map<CommentForAnswerGetModel>(comment);
        }

        public async Task EditAsync(CommentEditModel commentModel)
        {
            var comment = await GetCommentOnEditOrDeleteAsync(commentModel.ParentQuestionId, commentModel.ParentAnswerId, commentModel.CommentId);
            var user = await _userRepository.GetUser<User>(commentModel.UserId);
            comment.Edit(user, commentModel.Body, _limits);
            await _uow.SaveAsync();
            // @nl: raise an event?
        }

        public async Task DeleteAsync(CommentDeleteModel commentModel)
        {
            var comment = await GetCommentOnEditOrDeleteAsync(commentModel.ParentQuestionId, commentModel.ParentAnswerId, commentModel.CommentId);
            if (comment.Votes.Any() == true)
            {
                throw new BusinessException($"Cannot delete comment '{commentModel.CommentId}' because associated votes exist.");
            }
            _commentRepository.Delete(comment);
            await _uow.SaveAsync();
        }

        private async Task<Comment> GetCommentOnEditOrDeleteAsync(Guid? parentQuestionId, Guid? parentAnswerId, Guid commentId)
        {
            Comment comment = null;
            if (parentAnswerId.HasValue)
            {
                comment = await _commentRepository.GetCommentWithAnswerAsync(commentId);
                if (comment == null
                    || comment.ParentAnswerId != parentAnswerId
                    || comment.ParentAnswer.QuestionId != parentQuestionId)
                    throw new EntityNotFoundException(nameof(Comment), commentId);
            }
            else if (parentQuestionId.HasValue)
            {
                comment = await _commentRepository.GetCommentWithQuestionAsync(commentId);
                if (comment == null || comment.ParentQuestionId != parentQuestionId)
                    throw new EntityNotFoundException(nameof(Comment), commentId);
            }
            return comment;
        }

        public async Task DeleteRangeAsync(CommentsDeleteModel commentModel)
        {
            IEnumerable<Comment> comments = null;
            if (commentModel.ParentAnswerId.HasValue)
            {
                comments = await
                    _commentRepository.GetCommentsForAnswerAsync(commentModel.ParentAnswerId.Value);
            }
            else if (commentModel.ParentQuestionId.HasValue)
            {
                comments = await
                    _commentRepository.GetCommentsForQuestionAsync<Comment>(commentModel.ParentQuestionId.Value);
            }
            if (comments.Any(c => c.VotesSum > 0))
            {
                throw new BusinessException($"Cannot delete because associated votes exist on at least one comment.");
            }
            if (comments.Any())
            {
                _commentRepository.Delete(comments);
            }
        }
    }
}
