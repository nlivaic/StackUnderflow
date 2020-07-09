using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackUnderflow.Api.Constants;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Foo;
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
        public async Task<ActionResult<IEnumerable<QuestionSummaryGetViewModel>>> Get([FromQuery] QuestionResourceParameters questionResourceParameters)
        {
            var pagedSummaries = await _questionService.GetQuestionSummaries(questionResourceParameters);
            var pagingHeader = new PagingDto(pagedSummaries.CurrentPage, pagedSummaries.TotalPages, pagedSummaries.TotalItems);
            HttpContext.Response.Headers.Add(Headers.Pagination, new StringValues(JsonSerializer.Serialize(pagingHeader)));
            return Ok(pagedSummaries.Items);
        }
    }
}
