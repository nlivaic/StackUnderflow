using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.Models.Questions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Questions;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ApiControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public QuestionsController(
            IQuestionService questionService,
            IUserService userService,
            IMapper mapper)
        {
            _questionService = questionService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a single question.
        /// </summary>
        /// <returns>Question data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetQuestion")]
        public async Task<ActionResult<QuestionGetViewModel>> GetAsync([FromRoute]QuestionGetRequest questionGetRequest)
        {
            var questionFindModel = _mapper.Map<QuestionFindModel>(questionGetRequest);
            var question = await _questionService.GetQuestionWithUserAndTagsAsync(questionFindModel);
            if (question == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<QuestionGetViewModel>(question);
            response.IsOwner = User.IsOwner(response);
            response.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
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
            question.UserId = User.UserId().Value;
            var questionModel = await _questionService.AskQuestionAsync(question);
            var response = _mapper.Map<QuestionGetViewModel>(questionModel);
            response.IsOwner = true;
            response.IsModerator = User.Identity.IsAuthenticated && await _userService.IsModeratorAsync(User.UserId().Value);
            return CreatedAtRoute("GetQuestion", new { id = questionModel.Id }, response);
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
            question.QuestionUserId = User.UserId().Value;
            question.QuestionId = id;
            await _questionService.EditQuestionAsync(question);
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
            var userId = User.UserId().Value;
            await _questionService.DeleteQuestionAsync(id, userId);
            return NoContent();
        }
    }
}
