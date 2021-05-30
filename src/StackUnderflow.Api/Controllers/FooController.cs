using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Common.Interfaces;
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
            await _eventPublisher.PublishEvent<VoteCast>(new { Id = Guid.NewGuid() } );
            return Ok(1);
        }
    }
}
