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
    public class CommentService : ICommentService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly ILimits _limits;
        private readonly IVoteable _voteable;
        private readonly IMapper _mapper;

        public CommentService(IQuestionRepository questionRepository,
            ICommentRepository commentRepository,
            IAnswerRepository answerRepository,
            IRepository<User> userRepository,
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
            var comment = await GetCommentOnEditOrDelete(commentModel.ParentQuestionId, commentModel.ParentAnswerId, commentModel.CommentId);
            var user = await _userRepository.GetByIdAsync(commentModel.UserId);
            comment.Edit(user, commentModel.Body, _limits);
            await _uow.SaveAsync();
            // @nl: raise an event?
        }

        public async Task DeleteAsync(CommentDeleteModel commentModel)
        {
            var comment = await GetCommentOnEditOrDelete(commentModel.ParentQuestionId, commentModel.ParentAnswerId, commentModel.CommentId);
            if (comment.Votes.Any() == true)
            {
                throw new BusinessException($"Cannot delete comment '{commentModel.CommentId}' because associated votes exist.");
            }
            _commentRepository.Delete(comment);
            await _uow.SaveAsync();
        }

        private async Task<Comment> GetCommentOnEditOrDelete(Guid? parentQuestionId, Guid? parentAnswerId, Guid commentId)
        {
            Comment comment = null;
            if (parentAnswerId.HasValue)
            {
                comment = await _commentRepository.GetCommentWithAnswer(commentId);
                if (comment == null
                    || comment.ParentAnswerId != parentAnswerId
                    || comment.ParentAnswer.QuestionId != parentQuestionId)
                    throw new EntityNotFoundException(nameof(Comment), commentId);
            }
            else if (parentQuestionId.HasValue)
            {
                comment = await _commentRepository.GetByIdAsync(commentId);
                if (comment == null || comment.ParentQuestionId != parentQuestionId)
                    throw new EntityNotFoundException(nameof(Comment), commentId);
            }
            return comment;
        }
    }
}
