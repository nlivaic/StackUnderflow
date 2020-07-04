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
    public class QuestionSummariesController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionSummariesController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionSummaryGetViewModel>>> Get() =>
            Ok(await _questionService.GetQuestionSummaries());
    }
}
