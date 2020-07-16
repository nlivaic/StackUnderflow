using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackUnderflow.Api.Models;
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

        public QuestionsController(IQuestionService questionService, IMapper mapper, Microsoft.Extensions.Logging.ILogger<QuestionsController> logger)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetQuestion")]
        public async Task<ActionResult<QuestionGetViewModel>> Get(Guid id) =>
            Ok(await _questionService.GetQuestionWithUserAndTagsAsync(id));

        [HttpPost]
        public async Task<ActionResult<QuestionGetViewModel>> Post([FromBody] QuestionCreateRequest request)
        {
            var question = _mapper.Map<QuestionCreateModel>(request);
            question.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            var questionModel = await _questionService.AskQuestionAsync(question);
            return CreatedAtRoute("GetQuestion", new { id = questionModel.Id }, questionModel);
        }
    }
}
