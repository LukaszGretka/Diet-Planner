using DietPlanner.Api.Models.Account;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DietPlanner.Api.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager,
            ILogger<AccountService> logger)
        {
            _signInManager = signInManager;
            _userManager= userManager;
            _logger = logger;
        }

        public async Task<IdentityUser> GetUser(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogError($"user with email: {email} not found");
            }

            return user;
        }

        public async Task<SignInResult> LogIn(HttpContext httpContext, LogInRequest loginRequest)
        {
            var user = new IdentityUser(loginRequest.Email);


            return await _signInManager.PasswordSignInAsync(loginRequest.Email,
                loginRequest.Password, false, false);

            //if (!result.Succeeded)
            //{
            //    return result;
            //}

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, loginRequest.Email)
            //};

            //var claimsIdentity = new ClaimsIdentity(
            //        claims, CookieAuthenticationDefaults.AuthenticationScheme);



            //await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity));

            //return SignInResult.Success;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<DatabaseActionResult<IdentityUser>> SignUp(SignUpRequest signUpRequestData)
        {
            var user = new IdentityUser(signUpRequestData.Email);

            var createUserResult = await _userManager.CreateAsync(user, signUpRequestData.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError(string.Join(".", createUserResult.Errors));
                return new DatabaseActionResult<IdentityUser>(false, "error during user creation");
            }

            await _signInManager.SignInAsync(user, false);

            return new DatabaseActionResult<IdentityUser>(true, obj: user);
        }
    }
}
