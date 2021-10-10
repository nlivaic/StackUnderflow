using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Application.Users;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpGet("api/users/current")]
        public async Task<ActionResult<UserGetViewModel>> GetCurrent()
        {
            var userId = User.UserId().Value;
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserGetViewModel>(user));
        }

        [Authorize]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("api/users/current")]
        public async Task<ActionResult<UserGetViewModel>> PostCurrent([FromBody] UserCreateRequest request)
        {
            var model = _mapper.Map<UserCreateModel>(request);
            model.Id = User.UserId().Value;
            var user = await _userService.CreateUserAsync(model);
            return Ok(_mapper.Map<UserGetViewModel>(user));
        }
    }
}
