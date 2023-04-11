using DietPlanner.Api.Models;
using DietPlanner.Api.Services.SignUp;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            DatabaseActionResult<SignUpRequest> result = await _accountService.SignUp(signUpRequest);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
            //return CreatedAtAction(nameof(PostAsync), new { id = result.Obj.Id }, result.Obj);
        }

        [HttpPost("login")]
        public async Task<LogInResult> LogInAsync([FromBody] LogInRequest loginRequest)
        {
            LogInResult loginResult = await _accountService.LogIn(loginRequest);

            if (loginResult.Exception != null)
            {
                return new LogInResult(HttpStatusCode.InternalServerError);
            }

            return loginResult;
        }
    }
}
