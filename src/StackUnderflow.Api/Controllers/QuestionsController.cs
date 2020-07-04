using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionGetViewModel>> Get(Guid id) =>
            Ok(await _questionService.GetQuestionWithUserAndAllCommentsAsync(id));

        [HttpPost]
        public IActionResult Post([FromBody] QuestionCreateRequest request)
        {
            throw new NotImplementedException("");
        }
    }
}
