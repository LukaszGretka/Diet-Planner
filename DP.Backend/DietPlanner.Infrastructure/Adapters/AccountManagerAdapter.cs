using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Application.Models;
using DietPlanner.Application.Models.Account;
using DietPlanner.Domain.Constants;
using DietPlanner.Domain.Entities.User;
using DietPlanner.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Infrastructure.Adapters
{
    public class AccountManagerAdapter(SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager) : IAccountManagerAdapter
    {
        public async Task<ApplicationUser?> GetUserByName(string userName)
        {
            IdentityUser? identityUser = await userManager.FindByNameAsync(userName);

            return identityUser?.ToApplicationUser();
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            IdentityUser? identityUser = await userManager.FindByEmailAsync(email);

            return identityUser?.ToApplicationUser();
        }

        public async Task<SignInResult> PasswordSignInAsync(string username, string password)
        {
            return await signInManager.PasswordSignInAsync(username, password, false, false);
        }

        public Task Signout()
        {
            return signInManager.SignOutAsync();
        }

        public async Task<CreatedApplicationUser?> CreateUser(string userName, string email, string password)
        {
            var user = new IdentityUser(userName)
            {
                Email = email
            };

            IdentityResult? createUserResult = await userManager.CreateAsync(user, password);

            if (!createUserResult.Succeeded)
            {
                return null;
            }

            return new CreatedApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RequireEmailConfirmation = userManager.Options.SignIn.RequireConfirmedAccount
            };
        }

        public async Task<BaseResult> ConfirmUserEmail(string email, string confirmationToken)
        {
            IdentityUser? identityUser = await userManager.FindByEmailAsync(email);

            if (identityUser is null)
            {
                return BaseResult.Failed(ErrorCodes.UserNotFound);
            }

            await userManager.ConfirmEmailAsync(identityUser, confirmationToken);

            return BaseResult.Success;
        }

        public async Task<string> GenerateRegistrationTokenAsync(string userId)
        {
            IdentityUser? identityUser = await userManager.FindByIdAsync(userId);

            if (identityUser is null)
            {
                return string.Empty;
            }

            return await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        }

        public async Task<BaseResult> ChangePassword(ChangePasswordAction changePasswordRequest)
        {
            IdentityUser? user = await userManager.FindByIdAsync(changePasswordRequest.UserId);

            if (user is null)
            {
                return BaseResult.Failed(ErrorCodes.UserNotFound);
            }

            var currentPasswordValid = await userManager.CheckPasswordAsync(user, changePasswordRequest.CurrentPassword);

            if (!currentPasswordValid)
            {
                return BaseResult.Failed(ErrorCodes.InvalidPassword);
            }

            await userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);

            return BaseResult.Success;
        }
    }
}
