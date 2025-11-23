using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Application.Models;
using DietPlanner.Application.Models.Account;
using DietPlanner.Domain.Constants;
using DietPlanner.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Web;

namespace DietPlanner.Application.Services
{
    public class AccountService(
        IAccountManagerAdapter accountManagerAdapter,
        IMessageBrokerService messageBrokerService,
        ILogger<AccountService> logger,
        IConfiguration configuration) : IAccountService
    {
        public async Task<ApplicationUser?> GetUser(string userName)
        {
            ApplicationUser? user = await accountManagerAdapter.GetUserByName(userName);

            if (user is null)
            {
                logger.LogError("User with user name: {userName} not found", userName);
            }

            return user;
        }

        public async Task<SignInResult> SignIn(string userName, string password)
        {
            return await accountManagerAdapter.PasswordSignInAsync(userName, password);
        }

        public async Task Logout()
        {
            await accountManagerAdapter.Signout();
        }

        public async Task<SignUpResult> SignUp(string userName, string email, string password)
        {
            CreatedApplicationUser? createdUser = await accountManagerAdapter.CreateUser(userName, email, password);

            if (createdUser is null)
            {
                logger.LogError("Error during user creation");
                return new SignUpResult() { Succeeded = false, CreatedUser = null };
            }

            if (!createdUser.RequireEmailConfirmation)
            {
                await accountManagerAdapter.PasswordSignInAsync(userName, password);
            }
            else
            {
                string? token = await accountManagerAdapter.GenerateRegistrationTokenAsync(createdUser.Id);

                if(string.IsNullOrEmpty(token))
                {
                    logger.LogError("Error during email confirmation token generation for user: {userName}", userName);
                    return new SignUpResult() { Succeeded = false, CreatedUser = null };
                }

                string emailConfirmationLink = await CreateEmailRegistrationLink(createdUser);
                messageBrokerService.BroadcastSignUpEmail(createdUser.Email!, emailConfirmationLink);
            }

            return new SignUpResult() { CreatedUser = createdUser };
        }

        public async Task<BaseResult> ConfirmUserEmail(string email, string confirmationToken)
        {
            BaseResult result = await accountManagerAdapter.ConfirmUserEmail(email, confirmationToken);

            if (!result.Succeeded)
            {
                logger.LogError("Error during email confirmation for: {email}", email);
            }

            return result;
        }

        public async Task<BaseResult> ChangePassword(ChangePasswordAction changePasswordRequest)
        {
            if(changePasswordRequest.NewPassword != changePasswordRequest.ConfirmedNewPassword)
            {
                logger.LogError("New password and confirmed new password do not match for userId: {userId}", changePasswordRequest.UserId);
                return BaseResult.Failed(ErrorCodes.PasswordMismatch);
            }

            BaseResult changePasswordResult = await accountManagerAdapter.ChangePassword(changePasswordRequest);

            if (!changePasswordResult.Succeeded)
            {
                logger.LogError("Error during password change for userId: {userId}. Error code: {errorCode}",
                    changePasswordRequest.UserId, changePasswordResult.ErrorCode);
            }

            return changePasswordResult;
        }

        private async Task<string> CreateEmailRegistrationLink(ApplicationUser user)
        {
            string? emailConfirmationToken = await accountManagerAdapter.GenerateRegistrationTokenAsync(user.Id);

            if (string.IsNullOrEmpty(emailConfirmationToken))
            {
                logger.LogError("Error during email confirmation token generation for user: {userName}", user.UserName);
                return string.Empty;
            }

            var encodedConfirmationToken = HttpUtility.UrlEncode(emailConfirmationToken);

            string? spaHostAddress = configuration.GetSection("SpaConfig:HostAddress").Value;

            if (spaHostAddress is null)
            {
                logger.LogError("SPA host address is not configured. Cannot create email confirmation link.");
                return string.Empty;
            }

            return $"{spaHostAddress}/confirm-email?" +
                $"email={user.Email}&confirmationToken={encodedConfirmationToken}";
        }

    }
}
