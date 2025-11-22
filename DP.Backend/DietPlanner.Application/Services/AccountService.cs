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
        public async Task<IdentityUser?> GetUser(string userName)
        {
            IdentityUser? user = await userManager.FindByNameAsync(userName);

            if (user is null)
            {
                logger.LogError("User {UserName} not found", userName);
            }

            return user;
        }

        public async Task<SignInResult> SignIn(string userName, string password)
        {
            return await signInManager.PasswordSignInAsync(userName, password, false, false);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<SignupResult> SignUp(string userName, string email, string password)
        {
            var user = new IdentityUser(userName)
            {
                Email = email
            };

            IdentityResult createdUserResult = await userManager.CreateAsync(user, password);

            if (!createdUserResult.Succeeded)
            {
                logger.LogError(string.Join(".", createdUserResult.Errors));

                return (SignupResult) IdentityResult.Failed();
            }

            if (!userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                string? emailConfirmationLink = await CreateEmailRegistrationLink(user);

                if (string.IsNullOrEmpty(emailConfirmationLink))
                {
                    return (SignupResult) IdentityResult.Failed();
                }

                messageBrokerService.BroadcastSignUpEmail(user.Email, emailConfirmationLink);
            }

            return new SignupResult() 
            {
                RequireEmailConfirmation = userManager.Options.SignIn.RequireConfirmedAccount
            };
        }

        public async Task<IdentityResult> ConfirmUserEmail(string email, string confirmationToken)
        {
            IdentityUser? user = await userManager.FindByNameAsync(email);

            if (user is null)
            {
                logger.LogError($"Error during email confirmation for: {email}. User not found");
                return IdentityResult.Failed();
            }

            IdentityResult confirmEmailResult = await userManager.ConfirmEmailAsync(user, confirmationToken);

            if (!confirmEmailResult.Succeeded)
            {
                logger.LogError($"An error occured while confirming email address for {email}.");
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

            IdentityUser? user = await userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = $"Error during password change: {userName}. User not found"
                });
            }

            var currentPasswordValid = await userManager.CheckPasswordAsync(user, currentPassword);

            if(!currentPasswordValid)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidPassword",
                    Description = $"Error during password change: {userName}. Current password is not valid"
                });
            }

            return await userManager.ChangePasswordAsync(user,currentPassword, newPassword);
        }

        private async Task<string> CreateEmailRegistrationLink(IdentityUser user)
        {
            string emailConfirmationToken = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedConfirmationToken = HttpUtility.UrlEncode(emailConfirmationToken);

            string? spaHostAddress = configuration.GetSection("SpaConfig:HostAddress").Value;

            if(spaHostAddress is null)
            {
                logger.LogError("SPA host address is not configured. Cannot create email confirmation link.");
                return string.Empty;
            }

            return $"{spaHostAddress}/confirm-email?" +
                $"email={user.Email}&confirmationToken={encodedConfirmationToken}";
        }
    }
}
