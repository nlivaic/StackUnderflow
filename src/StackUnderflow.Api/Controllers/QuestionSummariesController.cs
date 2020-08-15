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
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public QuestionSummariesController(
            IQuestionService questionService,
            IQuestionRepository questionRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService)
        {
            _questionService = questionService;
            _questionRepository = questionRepository;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionSummaryGetViewModel>>> GetAsync([FromQuery] QuestionResourceParameters questionResourceParameters)
        {
            var questionQueryParameters = _mapper.Map<QuestionQueryParameters>(questionResourceParameters);
            var pagedSummaries = await _questionRepository.GetQuestionSummariesAsync(questionQueryParameters);
            var pagingHeader = new PagingDto(pagedSummaries.CurrentPage, pagedSummaries.TotalPages, pagedSummaries.TotalItems);
            HttpContext.Response.Headers.Add(Headers.Pagination, new StringValues(JsonSerializer.Serialize(pagingHeader)));
            return Ok(_mapper.Map<List<QuestionSummaryGetViewModel>>(pagedSummaries.Items));
        }
    }
}
