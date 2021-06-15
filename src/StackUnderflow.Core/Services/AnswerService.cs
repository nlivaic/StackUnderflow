using System.Threading.Tasks;
using AutoMapper;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ICommentService _commentService;

        public AnswerService(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task DeleteAnswerAsync(Answer answer, int votesSum)
        {
            answer.IsDeleteable(votesSum);
            await _commentService.DeleteRangeAsync(new CommentsDeleteModel
            {
                ParentAnswerId = answer.Id
            });
        }
    }
}
