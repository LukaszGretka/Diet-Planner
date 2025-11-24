using DietPlanner.Api.Extensions;
using DietPlanner.Api.Requests.Account;
using DietPlanner.Api.Responses;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Models.Account;
using DietPlanner.Domain.Constants;
using DietPlanner.Domain.Entities.Results;
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
    public class AccountController(IAccountService accountService, IValidator<SignUpRequest> signUpValidator) : Controller
    {
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpRequest request)
        {
            if (!signUpValidator.Validate(request).IsValid)
            {
                return BadRequest(ErrorCodes.ValidationFailed);
            }

            SignUpResult signUpResult = await accountService.SignUp(request.Username, request.Email, request.Password);

            if (!signUpResult.Succeeded)
            {
                return BadRequest(signUpResult.ErrorCode);
            }

            return Ok(new
            {
                User = new 
                {
                    username = signUpResult.CreatedUser.UserName, 
                },
                requireEmailConfirmation = signUpResult.CreatedUser.RequireEmailConfirmation
            });
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.ValidationFailed);
            }

            var signInResult = await accountService.SignIn(request.UserName, request.Password);

            if (!signInResult.Succeeded)
            {
                return Unauthorized(ErrorCodes.GeneralError);
            }

            return Ok(new
            {
                User = new { username = request.UserName },
                request.ReturnUrl
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserClaims()
        {
            var claims = HttpContext.User.Claims.ToList();

            return Ok(new
            {
                Username = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name))?.Value,
                Email = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Email))?.Value,
                UserId = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier))?.Value
            });
        }

        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> SignoutAsync()
        {
            await accountService.Logout();

            return Ok();
        }

        [Authorize]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmUserEmail([FromBody] EmailConfirmationRequest request)
        {
            BaseResult result = await accountService.ConfirmUserEmail(request.Email, request.ConfirmationToken);

            if(!result.Succeeded)
            {
                return BadRequest(result.ErrorCode.NormalizeErrorCode());
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            BaseResult result = await accountService.ChangePassword(new ChangePasswordAction() 
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword,
                ConfirmedNewPassword = request.NewPasswordConfirmed
            });

            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorCode.NormalizeErrorCode());
            }

            return Ok(result.Succeeded);
        }
    }
}
