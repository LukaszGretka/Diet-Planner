using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services.Account;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> GetUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("invalid email provided");
            }

            IdentityUser user = await _accountService.GetUser(email);

            if (user is null)
            {
                return NotFound("user not found");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return Ok(); 
        }
    }
}
