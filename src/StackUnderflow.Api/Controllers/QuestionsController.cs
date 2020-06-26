using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Interfaces;

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
        public ActionResult<Core.Models.QuestionModel> Get(Guid id)
        {
            return Ok(_questionService.GetQuestion(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] QuestionCreateRequest request)
        {
            throw new NotImplementedException("");
        }
    }
}
