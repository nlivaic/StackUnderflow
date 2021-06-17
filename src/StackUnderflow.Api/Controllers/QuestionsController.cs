using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.Models.Questions;
using StackUnderflow.Application.Questions.Commands;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Questions;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public QuestionsController(
            IQuestionService questionService,
            IUserService userService,
            ISender sender,
            IMapper mapper)
        {
            _questionService = questionService;
            _userService = userService;
            _sender = sender;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a single question.
        /// </summary>
        /// <returns>Question data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("{questionId}", Name = "GetQuestion")]
        public async Task<ActionResult<QuestionGetViewModel>> GetAsync([FromRoute]QuestionGetRequest questionGetRequest)
        {
            var getQuestionQuery = _mapper.Map<GetQuestionQuery>(questionGetRequest);
            var question = await _sender.Send(getQuestionQuery);
            var response = _mapper.Map<QuestionGetViewModel>(question);
            return Ok(response);
        }

        /// <summary>
        /// Create a new question [requires authentication].
        /// </summary>
        /// <param name="request">Question create body.</param>
        /// <returns>Newly created question.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<QuestionGetViewModel>> PostAsync([FromBody] QuestionCreateRequest request)
        {
            var createQuestionCommand = new CreateQuestionCommand
            {
                Title = request.Title,
                Body = request.Body,
                TagIds = request.TagIds,
                CurrentUserId = User.UserId().Value
            };
            var question = await _sender.Send(createQuestionCommand);
            var response = _mapper.Map<QuestionGetViewModel>(question);
            return CreatedAtRoute("GetQuestion", new { questionId = question.Id }, response);
        }

        /// <summary>
        /// Edit question. Question can be edited only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="request">Question edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPut("{questionId}")]
        public async Task<ActionResult> PutAsync([FromRoute]Guid questionId, [FromBody] QuestionUpdateRequest request)
        {
            var updateQuestionCommand = new UpdateQuestionCommand
            {
                CurrentUserId = User.UserId().Value,
                QuestionId = questionId,
                Title = request.Title,
                Body = request.Body,
                TagIds = request.TagIds
            };
            await _sender.Send(updateQuestionCommand);
            return NoContent();
        }

        /// <summary>
        /// Delete question. Question can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        [Authorize]
        [HttpDelete("{questionId}")]
        public async Task<ActionResult> DeleteAsync(Guid questionId)
        {
            var deleteQuestionCommand = new DeleteQuestionCommand
            {
                QuestionId = questionId,
                CurrentUserId = User.UserId().Value
            };
            await _sender.Send(deleteQuestionCommand);
            return NoContent();
        }
    }
}
