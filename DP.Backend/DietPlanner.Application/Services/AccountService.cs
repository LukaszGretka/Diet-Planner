using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Application.DTO.Identity;
using DietPlanner.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Web;

namespace DietPlanner.Application.Services
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

        public async Task<IdentityUser?> GetUser(string userName)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                _logger.LogError($"User with user name: {userName} not found");
            }

            return user;
        }

        public async Task<SignInResult> SignIn(string userName, string password)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, false, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SignupResult> SignUp(string userName, string email, string password)
        {
            var user = new IdentityUser(userName)
            {
                Email = email
            };

            IdentityResult createdUserResult = await _userManager.CreateAsync(user, password);

            if (!createdUserResult.Succeeded)
            {
                _logger.LogError(string.Join(".", createdUserResult.Errors));

                return (SignupResult) IdentityResult.Failed();
            }

            if (!_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                string? emailConfirmationLink = await CreateEmailRegistrationLink(user);

                if (string.IsNullOrEmpty(emailConfirmationLink))
                {
                    return (SignupResult) IdentityResult.Failed();
                }

                _messageBrokerService.BroadcastSignUpEmail(user.Email, emailConfirmationLink);
            }

            return new SignupResult() 
            {
                RequireEmailConfirmation = _userManager.Options.SignIn.RequireConfirmedAccount
            };
        }

        public async Task<IdentityResult> ConfirmUserEmail(string email, string confirmationToken)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(email);

            if (user is null)
            {
                _logger.LogError($"Error during email confirmation for: {email}. User not found");
                return IdentityResult.Failed();
            }

            IdentityResult confirmEmailResult = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            if (!confirmEmailResult.Succeeded)
            {
                _logger.LogError($"An error occured while confirming email address for {email}.");
            }

            return confirmEmailResult;
        }

        public async Task<IdentityResult> ChangePassword(string currentPassword, string newPassword, string newConfirmedPassword, string userName)
        {
            if(newPassword != newConfirmedPassword)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordMismatch",
                    Description = $"Error during password change for user {userName}. New password and confirm password do not match."
                });
            }

            IdentityUser? user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = $"Error during password change: {userName}. User not found"
                });
            }

            var currentPasswordValid = await _userManager.CheckPasswordAsync(user, currentPassword);

            if(!currentPasswordValid)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidPassword",
                    Description = $"Error during password change: {userName}. Current password is not valid"
                });
            }

            return await _userManager.ChangePasswordAsync(user,currentPassword, newPassword);
        }

        private async Task<string> CreateEmailRegistrationLink(IdentityUser user)
        {
            string emailConfirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedConfirmationToken = HttpUtility.UrlEncode(emailConfirmationToken);

            string? spaHostAddress = _configuration.GetSection("SpaConfig:HostAddress").Value;

            if(spaHostAddress is null)
            {
                _logger.LogError("SPA host address is not configured. Cannot create email confirmation link.");
                return string.Empty;
            }

            return $"{spaHostAddress}/confirm-email?" +
                $"email={user.Email}&confirmationToken={encodedConfirmationToken}";
        }
    }
}
