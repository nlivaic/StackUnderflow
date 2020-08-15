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
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentsController(
            ICommentRepository commentRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICommentService commentService,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet("api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<IEnumerable<CommentForQuestionGetViewModel>>> GetCommentForQuestion([FromRoute] Guid questionId)
        {
            if (!(await _questionRepository.ExistsAsync(questionId)))
            {
                return NotFound();
            }
            var comment = await _commentRepository.GetCommentsForQuestion(questionId);
            IEnumerable<CommentForQuestionGetViewModel> result = _mapper.Map<List<CommentForQuestionGetViewModel>>(comment);

            return Ok(result);
        }

        [HttpGet("api/questions/{questionId}/[controller]/{commentId}", Name = "GetCommentForQuestion")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> GetCommentForQuestion([FromRoute] Guid questionId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentModel(questionId, commentId);
            if (comment == null)
            {
                return NotFound();
            }
            CommentForQuestionGetViewModel result = _mapper.Map<CommentForQuestionGetViewModel>(comment);
            return Ok(result);
        }

        [HttpGet("/api/questions/{questionId}/answers/{answerIds}/[controller]", Name = "GetCommentsForAnswers")]
        public async Task<ActionResult<IEnumerable<CommentForAnswerGetViewModel>>> GetCommentsForAnswers(
            [FromRoute] Guid questionId,
            [FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> answerIds)
        {
            if (!(await _questionRepository.ExistsAsync(questionId)) || !(await _answerRepository.ExistsAsync(answerIds)))
            {
                return NotFound();
            }
            var comments = await _commentRepository.GetCommentsForAnswers(answerIds);
            var result = _mapper.Map<List<CommentForAnswerGetViewModel>>(comments);
            return Ok(result);
        }

        [HttpGet("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}", Name = "GetCommentForAnswer")]
        public async Task<ActionResult<CommentForAnswerGetViewModel>> GetCommentForAnswer(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentForAnswer(answerId, commentId);
            if (comment == null || comment.QuestionId != questionId)
                return NotFound();
            var result = _mapper.Map<CommentForAnswerGetViewModel>(comment);
            return Ok(result);
        }

        [HttpPost("/api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> PostOnQuestion([FromRoute] Guid questionId, [FromBody] CommentCreateRequest request)
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
            [FromBody] CommentCreateRequest request)
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
        public async Task<ActionResult> PutOnQuestion(
            [FromRoute] Guid questionId,
            [FromRoute] Guid commentId,
            [FromBody] UpdateCommentRequest request)
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

        [HttpPut("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}")]
        public async Task<ActionResult> PutOnAnswer(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromRoute] Guid commentId,
            [FromBody] UpdateCommentRequest request)
        {
            var comment = _mapper.Map<CommentEditModel>(request);
            comment.ParentQuestionId = questionId;
            comment.ParentAnswerId = answerId;
            comment.CommentId = commentId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
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
        public async Task<ActionResult> DeleteOnQuestion(
            [FromRoute] Guid questionId,
            [FromRoute] Guid commentId,
            [FromBody] UpdateCommentRequest request)
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

        [HttpDelete("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}")]
        public async Task<ActionResult> DeleteOnAnswer(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromRoute] Guid commentId)
        {
            var comment = new CommentDeleteModel
            {
                ParentQuestionId = questionId,
                ParentAnswerId = answerId,
                CommentId = commentId
            };
            try
            {
                await _commentService.DeleteAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
