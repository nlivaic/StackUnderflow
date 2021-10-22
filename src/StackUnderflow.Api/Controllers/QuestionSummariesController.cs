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
using StackUnderflow.Application.Sorting.Models;
using MediatR;
using StackUnderflow.Application.QuestionSummaries.Queries;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionSummariesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public QuestionSummariesController(
            ISender sender,
            IMapper mapper)
        {
            _sender = sender;
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
            var getQuestionSummariesQuery = new GetQuestionSummariesQuery
            {
                QuestionQueryParameters = questionQueryParameters
            };
            var pagedSummaries = await _sender.Send(getQuestionSummariesQuery);
            HttpContext.Response.Headers.Add(
                Headers.Pagination,
                new StringValues(JsonSerializer.Serialize(pagedSummaries.Paging)));
            return Ok(_mapper.Map<List<QuestionSummaryGetViewModel>>(pagedSummaries.Items));
        }
    }
}
