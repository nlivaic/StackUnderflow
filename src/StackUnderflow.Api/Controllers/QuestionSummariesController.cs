using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackUnderflow.Api.Constants;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.API.Services.Sorting;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionSummariesController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public QuestionSummariesController(IQuestionService questionService, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            _questionService = questionService;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionSummaryGetViewModel>>> Get([FromQuery] QuestionResourceParameters questionResourceParameters)
        {
            var questionQueryParameters = _mapper.Map<QuestionQueryParameters>(questionResourceParameters);
            var pagedSummaries = await _questionService.GetQuestionSummaries(questionQueryParameters);
            var pagingHeader = new PagingDto(pagedSummaries.CurrentPage, pagedSummaries.TotalPages, pagedSummaries.TotalItems);
            HttpContext.Response.Headers.Add(Headers.Pagination, new StringValues(JsonSerializer.Serialize(pagingHeader)));
            return Ok(pagedSummaries.Items);
        }
    }
}
