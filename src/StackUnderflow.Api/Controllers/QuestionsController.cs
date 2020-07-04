using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuestionResponse>> Get()
        {
            throw new NotImplementedException("");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionWithUserAndAllCommentsViewModel>> Get(Guid id)
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
