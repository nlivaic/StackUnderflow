using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackUnderflow.Api.Constants;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("/api/questions/{questionId}/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswersController(
            IAnswerService answerService,
            IAnswerRepository answerRepository,
            IMapper mapper)
        {
            _answerService = answerService;
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerGetViewModel>>> GetForQuestion(
            [FromRoute] Guid questionId,
            [FromQuery] AnswerResourceParameters answerResourceParameters)
        {
            var answerQueryParameters = _mapper.Map<AnswerQueryParameters>(answerResourceParameters);
            var pagedAnswers = await _answerRepository.GetAnswersWithUserAsync(questionId, answerQueryParameters);
            var pagingHeader = new PagingDto(pagedAnswers.CurrentPage, pagedAnswers.TotalPages, pagedAnswers.TotalItems);
            HttpContext.Response.Headers.Add(
                Headers.Pagination,
                new StringValues(JsonSerializer.Serialize(pagingHeader)));
            return Ok(_mapper.Map<List<AnswerGetViewModel>>(pagedAnswers.Items));
        }

        [HttpGet("{answerId}")]
        public async Task<ActionResult<AnswerGetViewModel>> Get(
            [FromRoute] Guid questionId,
            [FromRoute] Guid answerId)
        {
            var result = await _answerRepository.GetAnswerWithUserAsync(questionId, answerId);
            return Ok(_mapper.Map<AnswerGetViewModel>(result));
        }
    }
}
