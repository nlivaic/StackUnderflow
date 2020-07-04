using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuestionResponse>> Get()
        {
            throw new NotImplementedException("");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionWithUserAndAllCommentsModel>> Get(Guid id)
        {
            var q = await _questionService.GetQuestionWithUserAndAllCommentsAsync(id);
            return Ok(q);
        }

        [HttpPost]
        public IActionResult Post([FromBody] QuestionCreateRequest request)
        {
            throw new NotImplementedException("");
        }
    }
}
