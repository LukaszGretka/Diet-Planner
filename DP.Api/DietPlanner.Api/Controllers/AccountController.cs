﻿using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services.Account;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest signUpRequest)
        {
            DatabaseActionResult<IdentityUser> result = await _accountService.SignUp(signUpRequest);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!result.Success)
            {
                return BadRequest("signup_error");
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_input");
            }

            var signInResult = await _accountService.LogIn(HttpContext, loginRequest);

            if (!signInResult.Succeeded)
            {
                return Unauthorized("invalid_credential");
            }

            IdentityUser user = await _accountService.GetUser(loginRequest.Email);

            return Ok(new
            {
                User = new { username = user.UserName },
                ReturnUrl = loginRequest.ReturnUrl ?? string.Empty 
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            if (HttpContext.User is null)
            {
                return Unauthorized();
            }

            var claims = HttpContext.User.Claims.ToList();

            return Ok(new
            {
                Username = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name))?.Value
            });
        }

        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return Ok();
        }
    }
}