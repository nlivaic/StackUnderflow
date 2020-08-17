using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Api.Constants;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("/api/questions/{questionId}/[controller]")]
    public class AnswersController : ApiControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswersController(
            IAnswerService answerService,
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _answerService = answerService;
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all answers associated with the target question.
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerResourceParameters">Resource parameters allowing paging, ordering, searching and filtering.</param>
        /// <returns>List of answers.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
            var pagingHeader = new PagingDto(pagedAnswers.CurrentPage, pagedAnswers.TotalPages, pagedAnswers.TotalItems);
            HttpContext.Response.Headers.Add(
                Headers.Pagination,
                new StringValues(JsonSerializer.Serialize(pagingHeader)));
            return Ok(_mapper.Map<List<AnswerGetViewModel>>(pagedAnswers.Items));
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
            return Ok(_mapper.Map<AnswerGetViewModel>(result));
        }

        /// <summary>
        /// Create a new answer to the target question [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="request">Answer create body.</param>
        /// <returns>Newly created answer.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
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
            // Map answer to user fa11acfe-8234-4fa3-9733-19abe08f74e8 to force duplicate answer exception.
            // Map answer to user 59405a18-8f1d-4ece-a4cd-ef91d5bd65ae to create a new answer. Will work only on first attempt!
            answer.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            AnswerGetModel answerGetModel = null;
            try
            {
                answerGetModel = await _answerService.PostAnswerAsync(answer);
            }
            catch (BusinessException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return UnprocessableEntity();
            }
            var answerGetViewModel = _mapper.Map<AnswerGetViewModel>(answerGetModel);
            return CreatedAtRoute("Get", new { answerId = answerGetModel.Id, questionId }, answerGetViewModel);
        }

        /// <summary>
        /// Edit answer. Answer can be edited only a certain amount of time after it was created [requires authentication]. 
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <param name="request">Answer edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("{answerId}")]
        public async Task<ActionResult> PutAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId,
            [FromBody] AnswerUpdateRequest request)
        {
            var answer = _mapper.Map<AnswerEditModel>(request);
            answer.AnswerId = answerId;
            answer.QuestionId = questionId;
            answer.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");
            try
            {
                await _answerService.EditAnswerAsync(answer);
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
        /// Delete answer. Answer can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [HttpDelete("{answerId}")]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId)
        {
            var userId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");
            try
            {
                await _answerService.DeleteAnswerAsync(userId, questionId, answerId);
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
        /// Accept the answer to the target question [requires authentication].
        /// </summary>
        /// <param name="questionId">Question identifier.</param>
        /// <param name="answerId">Answer identifier.</param>
        /// <returns>Accepted answer.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [HttpPost("{answerId}/acceptAnswer")]
        public async Task<ActionResult<AnswerGetViewModel>> AcceptAnswer(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId)
        {
            var acceptAnswer = new AnswerAcceptModel
            {
                QuestionUserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8"),
                QuestionId = questionId,
                AnswerId = answerId
            };
            AnswerGetModel answerModel = null;
            try
            {
                answerModel = await _answerService.AcceptAnswerAsync(acceptAnswer);
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
            var answerViewModel = _mapper.Map<AnswerGetViewModel>(answerModel);
            return CreatedAtRoute("Get", new { questionId, answerId }, answerViewModel);
        }
    }
}
