using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Questions.Commands
{
    public class DeleteQuestionCommand : IRequest<Unit>
    {
        public Guid QuestionId { get; set; }
        public Guid CurrentUserId { get; set; }

        class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly IVoteRepository _voteRepository;
            private readonly IRepository<Comment> _commentRepository;

            public DeleteQuestionCommandHandler(
                IQuestionRepository questionRepository,
                IVoteRepository voteRepository,
                IRepository<Comment> commentRepository)
            {
                _questionRepository = questionRepository;
                _voteRepository = voteRepository;
                _commentRepository = commentRepository;
            }

            public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
            {
                var question = (await _questionRepository
                    .GetQuestionWithAnswersAndCommentsAsync(request.QuestionId));
                if (question == null || question.UserId != request.CurrentUserId)
                {
                    throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                }
                var votesSum = await _voteRepository.CountAsync(v => v.QuestionId == request.QuestionId);
                question.IsDeleteable();
                _commentRepository.Delete(question.Comments);
                _questionRepository.Delete(question);
                return Unit.Value;
            }
        }
    }
}
