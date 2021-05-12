﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models.Votes;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Votes;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ApiControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly IMapper _mapper;

        public VotesController(
            IVoteService voteService,
            IMapper mapper)
        {
            _voteService = voteService;
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
        public async Task<ActionResult<int>> PostAsync([FromBody]VoteCreateRequest model)
        {
            var voteCreateModel = _mapper.Map<VoteCreateModel>(model);
            VoteGetModel vote;
            try
            {
                vote = await _voteService.CastVoteAsync(voteCreateModel);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return UnprocessableEntity();
            }
            var voteResponseModel = _mapper.Map<VoteGetViewModel>(vote);
            return CreatedAtRoute("GetVote", new { voteId = vote.VoteId }, voteResponseModel);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{voteId}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync([FromRoute]Guid voteId)
        {
            try
            {
                await _voteService.RevokeVoteAsync(new VoteRevokeModel
                {
                    UserId = User.UserId().Value,
                    VoteId = voteId
                });
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}