using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.BaseControllers;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Events;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooController : ApiControllerBase
    {
        [HttpGet("1")]
        public async Task<ActionResult<int>> Get1()
        {
            throw new EntityNotFoundException("something not found.", Guid.NewGuid());
        }

        [HttpGet("2")]
        public async Task<ActionResult<int>> Get2()
        {
            throw new LimitNotMappable($"Limit not found, even though it is defined in database.");
        }

        [HttpGet("3")]
        public async Task<ActionResult<int>> Get3()
        {
            try
            {
                throw new BusinessException("Something bad happened.");
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return UnprocessableEntity();
            }
        }
    }
}
