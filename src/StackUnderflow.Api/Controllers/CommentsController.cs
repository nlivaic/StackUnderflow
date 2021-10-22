using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Application.Comments.Commands;
using StackUnderflow.Application.Comments.Queries;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public CommentsController(
            ISender sender,
            IMapper mapper)
        {
            _sender = sender;
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
            var comments = await _sender.Send(new GetCommentsForQuestionQuery
            {
                QuestionId = questionId,
                CurrentUserId = User.UserId()
            });
            var result = _mapper.Map<List<CommentForQuestionGetViewModel>>(comments);
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
            var comment = await _sender.Send(new GetCommentForQuestionQuery
            {
                QuestionId = questionId,
                CommentId = commentId,
                CurrentUserId = User.UserId()
            });
            var result = _mapper.Map<CommentForQuestionGetViewModel>(comment);
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
            var comments = await _sender.Send(new GetCommentsForAnswersQuery
            {
                QuestionId = questionId,
                AnswerIds = answerIds,
                CurrentUserId = User.UserId()
            });
            var result = _mapper.Map<List<CommentForAnswerGetViewModel>>(comments);
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
            var comment = await _sender.Send(new GetCommentForAnswerQuery
            {
                QuestionId = questionId,
                AnswerId = answerId,
                CommentId = commentId,
                CurrentUserId = User.UserId()
            });
            var result = _mapper.Map<CommentForAnswerGetViewModel>(comment);
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
            var createCommentOnQuestionCommand = new CreateCommentOnQuestionCommand
            {
                QuestionId = questionId,
                Body = request.Body,
                CurrentUserId = User.UserId().Value
            };
            var comment = await _sender.Send(createCommentOnQuestionCommand);
            var result = _mapper.Map<CommentForQuestionGetViewModel>(comment);
            return CreatedAtRoute("GetCommentForQuestion", new { questionId = questionId, commentId = comment.Id }, result);
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
            var createCommentOnAnswerCommand = new CreateCommentOnAnswerCommand
            {
                QuestionId = questionId,
                AnswerId = answerId,
                Body = request.Body,
                CurrentUserId = User.UserId().Value
            };
            var newCommentModel = await _sender.Send(createCommentOnAnswerCommand);
            var result = _mapper.Map<CommentForAnswerGetViewModel>(newCommentModel);
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
            var updateCommentOnQuestionCommand = new UpdateCommentOnQuestionCommand
            {
                ParentQuestionId = questionId,
                CommentId = commentId,
                CurrentUserId = User.UserId().Value,
                Body = request.Body
            };
            await _sender.Send(updateCommentOnQuestionCommand);
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
            var updateCommentOnAnswerCommand = new UpdateCommentOnAnswerCommand
            {
                ParentQuestionId = questionId,
                ParentAnswerId = answerId,
                CurrentUserId = User.UserId().Value,
                CommentId = commentId,
                Body = request.Body
            };
            await _sender.Send(updateCommentOnAnswerCommand);
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
            var deleteCommentOnQuestionCommand = new DeleteCommentOnQuestionCommand
            {
                ParentQuestionId = questionId,
                CommentId = commentId,
                CurrentUserId = User.UserId().Value
            };
            await _sender.Send(deleteCommentOnQuestionCommand);
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
            var deleteCommentOnAnswerCommand = new DeleteCommentOnAnswerCommand
            {
                ParentQuestionId = questionId,
                ParentAnswerId = answerId,
                CommentId = commentId,
                CurrentUserId = User.UserId().Value
            };
            await _sender.Send(deleteCommentOnAnswerCommand);
            return NoContent();
        }
    }
}
