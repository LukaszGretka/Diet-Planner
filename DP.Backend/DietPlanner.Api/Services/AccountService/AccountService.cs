using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace DietPlanner.Api.Services.AccountService
{
    public class AccountService(SignInManager<IdentityUser> signInManager,
        IMessageBrokerService messageBrokerService,
        UserManager<IdentityUser> userManager,
        ILogger<AccountService> logger,
        IConfiguration configuration) : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly ILogger<AccountService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<IdentityUser> GetUser(string userName)
        {
            IdentityUser user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                _logger.LogError($"User with user name: {userName} not found");
            }

            return user;
        }

        public async Task<SignInResult> SignIn(HttpContext httpContext, SignInRequest loginRequest)
        {
            return await _signInManager.PasswordSignInAsync(loginRequest.UserName,
                loginRequest.Password, false, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<DatabaseActionResult<SignUpResponse>> SignUp(SignUpRequest signUpRequestData)
        {
            var user = new IdentityUser(signUpRequestData.Username)
            {
                Email = signUpRequestData.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, signUpRequestData.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError(string.Join(".", createUserResult.Errors));
                return new DatabaseActionResult<SignUpResponse>(false, "error during user creation");
            }

            if (!_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else
            {
                var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedConfirmationToken = HttpUtility.UrlEncode(token);
                string spaHostAddress = _configuration.GetSection("SpaConfig:HostAddress").Value;
                string emailConfirmationLink = $"{spaHostAddress}/confirm-email?" +
                    $"email={user.Email}&confirmationToken={encodedConfirmationToken}";
                _messageBrokerService.BroadcastSignUpEmail(user.Email, emailConfirmationLink);
            }

            return new DatabaseActionResult<SignUpResponse>(true,
                obj: new SignUpResponse
                {
                    UserName = user.UserName,
                    RequireEmailConfirmation = _userManager.Options.SignIn.RequireConfirmedAccount
                });
        }

        public async Task<IdentityResult> ConfirmUserEmail(EmailConfirmationRequest emailConfirmationRequest)
        {
            IdentityUser user = await _userManager.FindByNameAsync(emailConfirmationRequest.Email);

            if (user is null)
            {
                _logger.LogError($"Error during email confirmation for: {emailConfirmationRequest.Email}. User not found");
                return new IdentityResult();
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, emailConfirmationRequest.ConfirmationToken);

            if (!confirmationResult.Succeeded)
            {
                confirmationResult.Errors.ToList().ForEach(error => _logger.LogError(error.Description));
            }

            return confirmationResult;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordRequest changePasswordRequest, IIdentity identity)
        {
            if(changePasswordRequest.NewPassword != changePasswordRequest.NewPasswordConfirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordMismatch",
                    Description = $"Error during password change: {identity.Name}. New password and confirm password do not match"
                });
            }

            IdentityUser user = await _userManager.FindByNameAsync(identity.Name);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = $"Error during password change: {identity.Name}. User not found"
                });
            }

            var currentPasswordValid = await _userManager.CheckPasswordAsync(user, changePasswordRequest.CurrentPassword);

            if(!currentPasswordValid)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidPassword",
                    Description = $"Error during password change: {identity.Name}. Current password is not valid"
                });
            }

            IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user,
                changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);

            return changePasswordResult;
        }
    }
}
