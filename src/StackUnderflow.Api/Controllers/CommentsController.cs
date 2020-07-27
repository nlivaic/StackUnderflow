using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Models;
using StackUnderflow.API.Helpers;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentsController(
            ICommentRepository commentRepository,
            ICommentService commentService,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet("api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<IEnumerable<CommentForQuestionGetViewModel>>> GetCommentForQuestion([FromRoute] Guid questionId)
        {
            var comment = await _commentRepository.GetCommentsForQuestion(questionId);
            IEnumerable<CommentForQuestionGetViewModel> result = _mapper.Map<List<CommentForQuestionGetViewModel>>(comment);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("api/questions/{questionId}/[controller]/{commentId}", Name = "GetCommentForQuestion")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> GetCommentsForQuestion([FromRoute] Guid questionId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentModel(questionId, commentId);
            if (comment == null)
            {
                return NotFound();
            }
            CommentForQuestionGetViewModel result = _mapper.Map<CommentForQuestionGetViewModel>(comment);
            return Ok(result);
        }

        [HttpGetAttribute("/api/questions/{questionId}/answers/{answerIds}/[controller]", Name = "GetCommentsForAnswers")]
        public async Task<ActionResult<IEnumerable<CommentForAnswerGetViewModel>>> GetCommentsForAnswers([FromRoute] Guid questionId,
            [FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> answerIds)
        {
            var comments = await _commentRepository.GetCommentsForAnswers(answerIds);
            var result = _mapper.Map<List<CommentForAnswerGetViewModel>>(comments);
            return Ok(result);
        }

        [HttpGetAttribute("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}", Name = "GetCommentForAnswer")]
        public async Task<ActionResult<CommentForAnswerGetViewModel>> GetCommentForAnswer([FromRoute] Guid questionId, [FromRoute] Guid answerId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentForAnswer(questionId, answerId, commentId);
            var result = _mapper.Map<CommentForAnswerGetViewModel>(comment);
            return Ok(result);
        }

        [HttpPost("/api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> PostOnQuestion([FromRoute] Guid questionId, [FromBody] CommentOnQuestionCreateRequest request)
        {
            var comment = _mapper.Map<CommentOnQuestionCreateModel>(request);
            comment.QuestionId = questionId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            CommentForQuestionGetModel commentModel = null;
            try
            {
                commentModel = await _commentService.CommentOnQuestionAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            var result = _mapper.Map<CommentForQuestionGetViewModel>(commentModel);
            return CreatedAtRoute("GetCommentForQuestion", new { questionId = questionId, commentId = commentModel.Id }, result);
        }

        [HttpPost("/api/questions/{questionId}/answers/{answerId}/[controller]")]
        public async Task<ActionResult<CommentForAnswerGetViewModel>> PostOnAnswer(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromBody] CommentOnAnswerCreateRequest request)
        {
            var comment = _mapper.Map<CommentOnAnswerCreateModel>(request);
            comment.QuestionId = questionId;
            comment.AnswerId = answerId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            CommentForAnswerGetModel result = null;
            try
            {
                result = await _commentService.CommentOnAnswerAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return CreatedAtRoute("GetCommentForAnswer", new { questionId, answerId, commentId = result.Id }, _mapper.Map<CommentForAnswerGetViewModel>(result));
        }

        [HttpPut("/api/questions/{questionId}/[controller]/{commentId}")]
        public async Task<ActionResult> PutOnQuestion([FromRoute] Guid questionId, [FromRoute] Guid commentId, [FromBody] UpdateCommentOnQuestionRequest request)
        {
            var comment = _mapper.Map<CommentEditModel>(request);
            comment.ParentQuestionId = questionId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            comment.CommentId = commentId;
            try
            {
                await _commentService.EditAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("/api/questions/{questionId}/[controller]/{commentId}")]
        public async Task<ActionResult> DeleteOnQuestion([FromRoute] Guid questionId, [FromRoute] Guid commentId, [FromBody] UpdateCommentOnQuestionRequest request)
        {
            try
            {
                await _commentService.DeleteAsync(new CommentDeleteModel { CommentId = commentId, ParentQuestionId = questionId });
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
