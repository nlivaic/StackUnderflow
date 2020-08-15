using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetQuestion")]
        public async Task<ActionResult<QuestionGetViewModel>> GetAsync(Guid id)
        {
            var question = await _questionService.GetQuestionWithUserAndTagsAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<QuestionGetViewModel>(question));
        }

        [HttpPost]
        public async Task<ActionResult<QuestionGetViewModel>> PostAsync([FromBody] QuestionCreateRequest request)
        {
            var question = _mapper.Map<QuestionCreateModel>(request);
            question.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            var questionModel = await _questionService.AskQuestionAsync(question);
            return CreatedAtRoute("GetQuestion", new { id = questionModel.Id }, _mapper.Map<QuestionGetViewModel>(questionModel));
        }

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
            return NoContent();
        }
    }
}
