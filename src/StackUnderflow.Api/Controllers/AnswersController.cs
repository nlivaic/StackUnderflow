using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackUnderflow.Api.Constants;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("/api/questions/{questionId}/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AnswersController(
            IAnswerService answerService,
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            IUserService userService,
            IMapper mapper)
        {
            _answerService = answerService;
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _userService = userService;
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
            if (!(await _questionRepository.ExistsAsync(questionId)))
            {
                return NotFound();
            }
            var pagedAnswers = await _answerRepository.GetAnswersWithUserAsync(questionId, answerQueryParameters);
            var pagingHeader = new PagingDto(
                pagedAnswers.CurrentPage,
                pagedAnswers.TotalPages,
                pagedAnswers.TotalItems,
                answerResourceParameters.PageSize > answerResourceParameters.MaximumPageSize
                    ? answerResourceParameters.MaximumPageSize
                    : answerResourceParameters.PageSize);
            HttpContext.Response.Headers.Add(
                Headers.Pagination,
                new StringValues(JsonSerializer.Serialize(pagingHeader)));
            var response = _mapper.Map<List<AnswerGetViewModel>>(pagedAnswers.Items);
            response.ForEach(async (a) =>
            {
                a.IsOwner = User.IsOwner(a);
                a.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            });
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
            var result = await _answerRepository.GetAnswerWithUserAsync(questionId, answerId);
            if (result == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<AnswerGetViewModel>(result);
            response.IsOwner = User.IsOwner(response);
            response.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
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
            if (!(await _questionRepository.ExistsAsync(questionId)))
            {
                return NotFound();
            }
            var answer = _mapper.Map<AnswerCreateModel>(request);
            answer.QuestionId = questionId;
            answer.UserId = User.UserId().Value;
            var answerGetModel = await _answerService.PostAnswerAsync(answer);
            var answerGetViewModel = _mapper.Map<AnswerGetViewModel>(answerGetModel);
            answerGetViewModel.IsOwner = User.IsOwner(answerGetViewModel);
            answerGetViewModel.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            return CreatedAtRoute("Get", new { answerId = answerGetModel.Id, questionId }, answerGetViewModel);
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
            var answer = _mapper.Map<AnswerEditModel>(request);
            answer.AnswerId = answerId;
            answer.QuestionId = questionId;
            answer.UserId = User.UserId().Value;
            await _answerService.EditAnswerAsync(answer);
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
            var userId = User.UserId().Value;
            await _answerService.DeleteAnswerAsync(userId, questionId, answerId);
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
            var acceptAnswer = new AnswerAcceptModel
            {
                QuestionUserId = User.UserId().Value,
                QuestionId = questionId,
                AnswerId = answerId
            };
            var answerModel = await _answerService.AcceptAnswerAsync(acceptAnswer);
            var answerViewModel = _mapper.Map<AnswerGetViewModel>(answerModel);
            answerViewModel.IsOwner = User.IsOwner(answerViewModel);
            return CreatedAtRoute("Get", new { questionId, answerId }, answerViewModel);
        }
    }
}
