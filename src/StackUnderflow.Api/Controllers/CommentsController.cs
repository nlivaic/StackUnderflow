using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    public class CommentsController : ApiControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CommentsController(
            ICommentRepository commentRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICommentService commentService,
            IUserService userService,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentService = commentService;
            _userService = userService;
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
            var comments = await _commentRepository.GetCommentsForQuestionAsync<CommentForQuestionGetModel>(questionId);
            List<CommentForQuestionGetViewModel> result = _mapper.Map<List<CommentForQuestionGetViewModel>>(comments);
            result.ForEach(async (comment) =>
            {
                comment.IsOwner = User.IsOwner(comment);
                comment.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            });
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
            result.IsOwner = User.IsOwner(result);
            result.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
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
            result.ForEach(async (comment) =>
            {
                comment.IsOwner = User.IsOwner(comment);
                comment.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            });
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
            result.IsOwner = User.IsOwner(result);
            result.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            return Ok(result);
        }

        /// <summary>
        /// Create a new comment to the target question [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="request">Comment create body.</param>
        /// <returns>Newly created comment.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPost("/api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<CommentForQuestionGetViewModel>> PostOnQuestionAsync([FromRoute] Guid questionId, [FromBody] CommentCreateRequest request)
        {
            var comment = _mapper.Map<CommentOnQuestionCreateModel>(request);
            comment.QuestionId = questionId;
            comment.UserId = User.UserId().Value;
            CommentForQuestionGetModel newCommentModel = null;
            try
            {
                newCommentModel = await _commentService.CommentOnQuestionAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            var result = _mapper.Map<CommentForQuestionGetViewModel>(newCommentModel);
            result.IsOwner = User.IsOwner(result);
            result.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            return CreatedAtRoute("GetCommentForQuestion", new { questionId = questionId, commentId = newCommentModel.Id }, result);
        }

        /// <summary>
        /// Create a new comment to the target answer [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="request">Comment create body.</param>
        /// <returns>Newly created comment.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPost("/api/questions/{questionId}/answers/{answerId}/[controller]")]
        public async Task<ActionResult<CommentForAnswerGetViewModel>> PostOnAnswerAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromBody] CommentCreateRequest request)
        {
            var comment = _mapper.Map<CommentOnAnswerCreateModel>(request);
            comment.QuestionId = questionId;
            comment.AnswerId = answerId;
            comment.UserId = User.UserId().Value;
            CommentForAnswerGetModel newCommentModel = null;
            try
            {
                newCommentModel = await _commentService.CommentOnAnswerAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            var result = _mapper.Map<CommentForAnswerGetViewModel>(newCommentModel);
            result.IsOwner = User.IsOwner(result);
            result.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            return CreatedAtRoute("GetCommentForAnswer", new { questionId, answerId, commentId = result.Id }, result);
        }

        /// <summary>
        /// Edit comment on target question. Comment can be edited only a certain amount of time after it was created [requires authentication]. 
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        /// <param name="request">Comment edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPut("/api/questions/{questionId}/[controller]/{commentId}")]
        public async Task<ActionResult> PutOnQuestionAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid commentId,
            [FromBody] UpdateCommentRequest request)
        {
            var comment = _mapper.Map<CommentEditModel>(request);
            comment.ParentQuestionId = questionId;
            comment.UserId = User.UserId().Value;
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
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
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
            comment.UserId = User.UserId().Value;
            try
            {
                await _commentService.EditAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return UnprocessableEntity();
            }
            return NoContent();
        }

        /// <summary>
        /// Delete comment on target question. Comment can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="commentId">Comment identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Authorize]
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
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Authorize]
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
