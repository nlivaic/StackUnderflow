using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Api.Models;
using StackUnderflow.API.Helpers;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    public class CommentsController : ApiControllerBase
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

        /// <summary>
        /// Get all comments associated with the target question.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <returns>List of comments.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<IEnumerable<CommentForQuestionGetViewModel>>> GetCommentsForQuestionAsync([FromRoute] Guid questionId)
        {
            if (!(await _questionRepository.ExistsAsync(questionId)))
            {
                return NotFound();
            }
            var comment = await _commentRepository.GetCommentsForQuestionAsync(questionId);
            List<CommentForQuestionGetViewModel> result = _mapper.Map<List<CommentForQuestionGetViewModel>>(comment);
            result.ForEach(comment => comment.IsOwner = Foo.TemporaryUser.Get == comment.UserId);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific comment for a target question.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        /// <returns>Single comment.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("api/questions/{questionId}/[controller]/{commentId}", Name = "GetCommentForQuestion")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> GetCommentForQuestionAsync([FromRoute] Guid questionId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentModelAsync(questionId, commentId);
            if (comment == null)
            {
                return NotFound();
            }
            CommentForQuestionGetViewModel result = _mapper.Map<CommentForQuestionGetViewModel>(comment);
            result.IsOwner = Foo.TemporaryUser.Get == comment.UserId;
            return Ok(result);
        }

        /// <summary>
        /// Get all comments associated with multiple target answers.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerIds">List of answer identifiers.</param>
        /// <returns>List of comments.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("/api/questions/{questionId}/answers/{answerIds}/[controller]", Name = "GetCommentsForAnswers")]
        public async Task<ActionResult<IEnumerable<CommentForAnswerGetViewModel>>> GetCommentsForAnswersAsync(
            [FromRoute] Guid questionId,
            [FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> answerIds)
        {
            if (!(await _questionRepository.ExistsAsync(questionId)) || !(await _answerRepository.ExistsAsync(answerIds)))
            {
                return NotFound();
            }
            var comments = await _commentRepository.GetCommentsForAnswersAsync(answerIds);
            var result = _mapper.Map<List<CommentForAnswerGetViewModel>>(comments);
            result.ForEach(comment => comment.IsOwner = Foo.TemporaryUser.Get == comment.UserId);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific comment for a target answer.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        /// <returns>Single comment.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}", Name = "GetCommentForAnswer")]
        public async Task<ActionResult<CommentForAnswerGetViewModel>> GetCommentForAnswerAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentForAnswerAsync(answerId, commentId);
            if (comment == null || comment.QuestionId != questionId)
                return NotFound();
            var result = _mapper.Map<CommentForAnswerGetViewModel>(comment);
            result.IsOwner = Foo.TemporaryUser.Get == comment.UserId;
            return Ok(result);
        }

        /// <summary>
        /// Create a new comment to the target question [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="request">Comment create body.</param>
        /// <returns>Newly created comment.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("/api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> PostOnQuestionAsync([FromRoute] Guid questionId, [FromBody] CommentCreateRequest request)
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

        /// <summary>
        /// Create a new comment to the target answer [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="request">Comment create body.</param>
        /// <returns>Newly created comment.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("/api/questions/{questionId}/answers/{answerId}/[controller]")]
        public async Task<ActionResult<CommentForAnswerGetViewModel>> PostOnAnswerAsync(
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

        /// <summary>
        /// Edit comment on target question. Comment can be edited only a certain amount of time after it was created [requires authentication]. 
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        /// <param name="request">Comment edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Consumes("application/json")]
        [HttpPut("/api/questions/{questionId}/[controller]/{commentId}")]
        public async Task<ActionResult> PutOnQuestionAsync(
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
            catch (BusinessException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return UnprocessableEntity();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Edit comment on target answer. Comment can be edited only a certain amount of time after it was created [requires authentication]. 
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        /// <param name="request">Comment edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Consumes("application/json")]
        [HttpPut("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}")]
        public async Task<ActionResult> PutOnAnswerAsync(
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

        /// <summary>
        /// Delete comment on target question. Comment can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("/api/questions/{questionId}/[controller]/{commentId}")]
        public async Task<ActionResult> DeleteOnQuestionAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid commentId)
        {
            try
            {
                await _commentService.DeleteAsync(new CommentDeleteModel { CommentId = commentId, ParentQuestionId = questionId });
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Conflict(ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete comment on target answer. Comment can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("/api/questions/{questionId}/answers/{answerId}/[controller]/{commentId}")]
        public async Task<ActionResult> DeleteOnAnswerAsync(
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
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Conflict(ModelState);
            }
            return NoContent();
        }
    }
}
