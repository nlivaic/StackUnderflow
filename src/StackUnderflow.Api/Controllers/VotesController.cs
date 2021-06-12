using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Models.Votes;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly IRegisteredEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public VotesController(
            IVoteService voteService,
            IRegisteredEventPublisher eventPublisher,
            IMapper mapper)
        {
            _voteService = voteService;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a single vote. This is a bare bones return value, no real need for it
        /// except to fulfill a RESTful contract.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet(Name = "GetVote")]
        public async Task<ActionResult<VoteGetViewModel>> GetVoteAsync([FromRoute] Guid voteId)
        {
            var vote = await _voteService.GetVoteAsync(voteId);
            if (vote != null)
            {
                return NotFound();
            }
            var voteModel = _mapper.Map<VoteGetViewModel>(vote);
            return Ok(voteModel);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VoteGetViewModel>> PostAsync([FromBody]VoteCreateRequest model)
        {
            var voteCreateModel = _mapper.Map<VoteCreateModel>(model);
            var vote = await _voteService.CastVoteAsync(voteCreateModel);
            await _eventPublisher.PublishAll();
            var voteResponseModel = _mapper.Map<VoteGetViewModel>(vote);
            return CreatedAtRoute("GetVote", new { voteId = vote.Id }, voteResponseModel);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{voteId}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync([FromRoute]VoteDeleteRequest voteDeleteRequest)
        {
            var voteRevokeModel = _mapper.Map<VoteRevokeModel>(voteDeleteRequest);
            await _voteService.RevokeVoteAsync(voteRevokeModel);
            return NoContent();
        }
    }
}
