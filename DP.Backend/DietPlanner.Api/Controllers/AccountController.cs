using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services.Account;
using DietPlanner.Api.Validators;
using DietPlanner.Shared.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<SignUpRequest> _signUpValidator;

        public AccountController(IAccountService accountService, IValidator<SignUpRequest> signUpValidator)
        {
            _accountService = accountService;
            _signUpValidator = signUpValidator;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
        {
            if (!_signUpValidator.Validate(signUpRequest).IsValid)
            {
                return BadRequest("signup_error");
            }

            DatabaseActionResult<SignUpResponse> result = await _accountService.SignUp(signUpRequest);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!result.Success)
            {
                return BadRequest("signup_error");
            }

            return Ok(new
            {
                User = new 
                {
                    username = result.Obj.UserName, 
                },
                requireEmailConfirmation = result.Obj.RequireEmailConfirmation
            });
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_input");
            }

            var signInResult = await _accountService.SignIn(HttpContext, loginRequest);

            if (!signInResult.Succeeded)
            {
                return Unauthorized("invalid_credential");
            }

            IdentityUser user = await _accountService.GetUser(loginRequest.Email);

            return Ok(new
            {
                User = new { username = user.UserName },
                loginRequest.ReturnUrl
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserClaims()
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
        public async Task<IActionResult> SignoutAsync()
        {
            await _accountService.Logout();

            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmUserEmail([FromBody] EmailConfirmationRequest emailConfirmationRequest)
        {
            var result = await _accountService.ConfirmUserEmail(emailConfirmationRequest);

            if(!result.Succeeded)
            {
                return BadRequest("unable_to_confirm_user_email");
            }

            return Ok();
        }
    }
}
