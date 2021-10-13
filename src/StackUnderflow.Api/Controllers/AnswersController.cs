using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackUnderflow.Api.Constants;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Application.Answers.Commands;
using StackUnderflow.Application.Answers.Queries;
using StackUnderflow.Core.Sorting.Models;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("/api/questions/{questionId}/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public AnswersController(
            ISender sender,
            IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all answers associated with the target question.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerResourceParameters">Resource parameters allowing paging, ordering, searching and filtering.</param>
        /// <returns>List of answers.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerGetViewModel>>> GetForQuestionAsync(
            [FromRoute] Guid questionId,
            [FromQuery] AnswerResourceParameters answerResourceParameters)
        {
            var answerQueryParameters = _mapper.Map<AnswerQueryParameters>(answerResourceParameters);
            var pagedAnswers = await _sender.Send(new GetAnswersQuery(
                questionId,
                answerQueryParameters,
                User.UserId()));
            HttpContext.Response.Headers.Add(
                Headers.Pagination,
                new StringValues(JsonSerializer.Serialize(pagedAnswers.Paging)));
            var response = _mapper.Map<List<AnswerGetViewModel>>(pagedAnswers.Items);
            return Ok(response);
        }

        /// <summary>
        /// Get a single answer associated with the target question.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <returns>Single answer.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("{answerId}", Name = "Get")]
        public async Task<ActionResult<AnswerGetViewModel>> GetAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId)
        {
            var getAnswerQuery = new GetAnswerQuery
            {
                QuestionId = questionId,
                AnswerId = answerId,
                CurrentUserId = User.UserId()
            };
            var result = await _sender.Send(getAnswerQuery);
            var response = _mapper.Map<AnswerGetViewModel>(result);
            return Ok(response);
        }

        /// <summary>
        /// Create a new answer to the target question [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="request">Answer create body.</param>
        /// <returns>Newly created answer.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AnswerGetViewModel>> PostAsync(
            [FromRoute] Guid questionId,
            [FromBody] AnswerCreateRequest request)
        {
            var createAnswerCommand = new CreateAnswerCommand
            {
                QuestionId = questionId,
                UserId = User.UserId().Value,
                Body = request.Body
            };
            var result = await _sender.Send(createAnswerCommand);
            var answerGetViewModel = _mapper.Map<AnswerGetViewModel>(result);
            return CreatedAtRoute("Get", new { answerId = result.Id, questionId }, answerGetViewModel);
        }

        /// <summary>
        /// Edit answer. Answer can be edited only a certain amount of time after it was created [requires authentication]. 
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="request">Answer edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPut("{answerId}")]
        public async Task<ActionResult> PutAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromBody] AnswerUpdateRequest request)
        {
            var updateAnswerCommand = new UpdateAnswerCommand
            {
                CurrentUserId = User.UserId().Value,
                QuestionId = questionId,
                AnswerId = answerId,
                Body = request.Body
            };
            await _sender.Send(updateAnswerCommand);
            return NoContent();
        }

        /// <summary>
        /// Delete answer. Answer can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Authorize]
        [HttpDelete("{answerId}")]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId)
        {
            await _sender.Send(new DeleteAnswerCommand
            {
                QuestionId = questionId,
                AnswerId = answerId,
                CurrentUserId = User.UserId().Value
            });
            return NoContent();
        }

        /// <summary>
        /// Accept the answer to the target question [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <returns>Accepted answer.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPost("{answerId}/acceptAnswer")]
        public async Task<ActionResult<AnswerGetViewModel>> AcceptAnswer(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId)
        {
            var acceptAnswer = new AcceptAnswerCommand
            {
                CurrentUserId = User.UserId().Value,
                QuestionId = questionId,
                AnswerId = answerId
            };
            var answerModel = await _sender.Send(acceptAnswer);
            var answerViewModel = _mapper.Map<AnswerGetViewModel>(answerModel);
            return CreatedAtRoute("Get", new { questionId, answerId }, answerViewModel);
        }
    }
}
