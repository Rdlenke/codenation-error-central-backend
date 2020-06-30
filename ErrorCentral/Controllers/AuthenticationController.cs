using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorCentral.Contracts;
using ErrorCentral.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Authentication.Register)]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            AuthenticationResponse response = await _userService.RegisterAsync(registerRequest);

            if (!response.Sucess)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Authentication.Login)]
        public async Task<IActionResult> Login([FromBody]AuthenticationRequest authenticationRequest)
        {
            AuthenticationResponse response = await _userService.AuthenticateAsync(authenticationRequest);

            if (!response.Sucess)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response);
        }
    }
}
