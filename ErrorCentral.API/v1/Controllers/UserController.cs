using System;
using System.Net;
using System.Threading.Tasks;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Domain.SeedWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErrorCentral.API.v1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserViewModel createUserViewModel)
        {
            Response<GetUserViewModel> response = await _userService.CreateAsync(createUserViewModel);

            if(!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody]AuthenticateUserViewModel authenticateUserViewModel)
        {
            Response<GetUserViewModel> response = await _userService.AuthenticateAsync(authenticateUserViewModel);

            if(!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }
    }
}
