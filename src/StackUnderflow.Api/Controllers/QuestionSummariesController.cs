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
using StackUnderflow.Application.Services.Sorting;
using StackUnderflow.Application.Services.Sorting.Models;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionSummariesController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionSummariesController(
            IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
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
            var pagingHeader = new Paging(
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
