using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ApiControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a single question.
        /// </summary>
        /// <param name="id">Question identifier</param>
        /// <returns>Question data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetQuestion")]
        public async Task<ActionResult<QuestionGetViewModel>> GetAsync([FromRoute] Guid id)
        {
            var question = await _questionService.GetQuestionWithUserAndTagsAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<QuestionGetViewModel>(question);
            response.IsOwner = User.IsOwner(response);
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
            var question = _mapper.Map<QuestionCreateModel>(request);
            question.UserId = User.Claims.UserId();
            var questionModel = await _questionService.AskQuestionAsync(question);
            return CreatedAtRoute("GetQuestion", new { id = questionModel.Id }, _mapper.Map<QuestionGetViewModel>(questionModel));
        }

        /// <summary>
        /// Edit question. Question can be edited only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="id">Question identifier.</param>
        /// <param name="request">Question edit data.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] Guid id, [FromBody] QuestionUpdateRequest request)
        {
            var question = _mapper.Map<QuestionEditModel>(request);
            question.QuestionUserId = User.Claims.UserId();
            question.QuestionId = id;
            try
            {
                await _questionService.EditQuestionAsync(question);
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
        /// Delete question. Question can be deleted only a certain amount of time after it was created [requires authentication].
        /// </summary>
        /// <param name="id">Question identifier.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var userId = User.Claims.UserId();
            try
            {
                await _questionService.DeleteQuestionAsync(id, userId);
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
