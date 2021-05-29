using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public FooController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        [HttpGet]
        public async Task<ActionResult<int>> Get()
        {
            await _publishEndpoint.Publish<SomeEventHappened>(new { Id = Guid.NewGuid() } );
            return Ok(1);
        }
    }
    public interface SomeEventHappened
    {
        public int Id { get; set; }
    }

    public class SomeEventHappenedConsumer : IConsumer<SomeEventHappened>
    {
        public Task Consume(ConsumeContext<SomeEventHappened> context)
        {
            //throw new Exception("Bad things happened in consumer.");
            return Task.CompletedTask;
        }
    }
}
