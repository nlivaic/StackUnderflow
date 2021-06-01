using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Events;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;

        public FooController(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        [HttpGet]
        public async Task<ActionResult<int>> Get()
        {
            await _eventPublisher.PublishEvent<VoteCast>(
                new
                {
                    UserId = Guid.Parse("1e392be2-e621-4ac1-86c3-81aa7f4873ea"),
                    VoteType = VoteTypeEnum.Upvote
                });
            return Ok(1);
        }
    }
}
