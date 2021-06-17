using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using AutoMapper;

namespace StackUnderflow.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ICommentService _commentService;

        public QuestionService(
            ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task DeleteQuestionAsync(Question question, int votesSum)
        {
            question.IsDeleteable(votesSum);
            await _commentService.DeleteRangeAsync(new CommentsDeleteModel
            {
                ParentQuestionId = question.Id
            });
        }
    }
}
