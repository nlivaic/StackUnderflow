using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// A list of question summaries (a brief overview of each question).
        /// </summary>
        /// <param name="questionSummaryResourceParameters">Resource parameters allowing paging, ordering, searching and filtering.</param>
        /// <returns>A list of question summaries.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionSummaryGetViewModel>>> GetAsync([FromQuery] QuestionSummaryResourceParameters questionSummaryResourceParameters)
        {
            var questionQueryParameters = _mapper.Map<QuestionQueryParameters>(questionSummaryResourceParameters);
            var pagedSummaries = await _questionRepository.GetQuestionSummariesAsync(questionQueryParameters);
            var pagingHeader = new PagingDto(
                pagedSummaries.CurrentPage,
                pagedSummaries.TotalPages,
                pagedSummaries.TotalItems,
                questionSummaryResourceParameters.PageSize > questionSummaryResourceParameters.MaximumPageSize
                    ? questionSummaryResourceParameters.MaximumPageSize
                    : questionSummaryResourceParameters.PageSize);
            HttpContext.Response.Headers.Add(Headers.Pagination, new StringValues(JsonSerializer.Serialize(pagingHeader)));
            return Ok(_mapper.Map<List<QuestionSummaryGetViewModel>>(pagedSummaries.Items));
        }
    }
}
