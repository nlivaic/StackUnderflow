using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.BaseControllers;
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
            return Ok(_mapper.Map<QuestionGetViewModel>(question));
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<QuestionGetViewModel>> PostAsync([FromBody] QuestionCreateRequest request)
        {
            var question = _mapper.Map<QuestionCreateModel>(request);
            question.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            var questionModel = await _questionService.AskQuestionAsync(question);
            return CreatedAtRoute("GetQuestion", new { id = questionModel.Id }, _mapper.Map<QuestionGetViewModel>(questionModel));
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromRoute] Guid id, [FromBody] QuestionUpdateRequest request)
        {
            var question = _mapper.Map<QuestionEditModel>(request);
            question.QuestionUserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            question.QuestionId = id;
            try
            {
                await _questionService.EditQuestionAsync(question);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var userId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
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
