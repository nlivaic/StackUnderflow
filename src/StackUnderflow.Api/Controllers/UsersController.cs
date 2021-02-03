using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
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
        public async Task<ActionResult<UserGetViewModel>> Get()
        {
            var userId = User.Claims.UserId();
            var user = await _userService.GetUserAsync(userId);
            return _mapper.Map<UserGetViewModel>(user);
        }
    }
}
