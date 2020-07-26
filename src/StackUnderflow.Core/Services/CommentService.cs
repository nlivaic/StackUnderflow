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
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly ILimits _limits;
        private readonly IVoteable _voteable;
        private readonly IMapper _mapper;

        public CommentService(IQuestionRepository questionRepository,
            ICommentRepository commentRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork,
            ILimits limits,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _uow = unitOfWork;
            _limits = limits;
            _voteable = new Voteable();
            _mapper = mapper;
        }

        public async Task<CommentGetModel> CommentOnQuestionAsync(CommentOnQuestionCreateModel commentModel)
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
            return _mapper.Map<CommentGetModel>(comment);
        }

        public async Task EditAsync(CommentEditModel commentModel)
        {
            var comment = (await _commentRepository.GetCommentWithUser(commentModel.CommentId))
                ?? throw new EntityNotFoundException(nameof(Comment), commentModel.CommentId);
            if (comment.ParentQuestionId != commentModel.ParentQuestionId)
            {
                throw new EntityNotFoundException(nameof(Comment), commentModel.CommentId);
            }
            var user = await _userRepository.GetByIdAsync(commentModel.UserId);
            comment.Edit(user, commentModel.Body, _limits);
            await _uow.SaveAsync();
            // @nl: raise an event?
        }
    }
}
