using DietPlanner.Api.Requests;
using DietPlanner.Application.DTO.Identity;
using DietPlanner.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService, 
        IValidator<SignUpRequest> signUpValidator) : Controller
    {

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            if (!signUpValidator.Validate(request).IsValid)
            {
                return BadRequest("signup_error");
            }

            SignupResult signupResult = await accountService.SignUp(request.Username, request.Email, request.Password);

            if (!signupResult.Succeeded)
            {
                return BadRequest("signup_error");
            }

            return Ok(new
            {
                User = new 
                {
                    username = request.Username, 
                },
                requireEmailConfirmation = signupResult.RequireEmailConfirmation
            });
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_input");
            }

            SignInResult signInResult = await accountService.SignIn(request.UserName, request.Password);

            if (!signInResult.Succeeded)
            {
                return Unauthorized("invalid_credential");
            }

            IdentityUser user = await accountService.GetUser(request.UserName);

            return Ok(new
            {
                User = new { username = user.UserName },
                request.ReturnUrl
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
            await accountService.Logout();

            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmUserEmail([FromBody] EmailConfirmationRequest request)
        {
            IdentityResult result = await accountService.ConfirmUserEmail(request.Email, request.ConfirmationToken);

            if (!result.Succeeded)
            {
                return BadRequest("unable_to_confirm_user_email");
            }

            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (HttpContext.User is null)
            {
                return Unauthorized();
            }

            IdentityResult result = await accountService.ChangePassword(request.CurrentPassword, 
                request.NewPassword, request.NewPasswordConfirmed, HttpContext.User.Identity.Name);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault().Code);
            }

            return Ok(result.Succeeded);
        }
    }
}
