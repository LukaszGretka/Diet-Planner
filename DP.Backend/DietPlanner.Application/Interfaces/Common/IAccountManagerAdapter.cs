using DietPlanner.Application.Models.Account;
using DietPlanner.Domain.Entities.Account;
using DietPlanner.Domain.Entities.Results;
using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Application.Interfaces.Common
{
    public interface IAccountManagerAdapter
    {
        Task<ApplicationUser?> GetUserByName(string username);

        Task<ApplicationUser?> GetUserByEmail(string email);

        Task<SignInResult> PasswordSignInAsync(string username, string password);

        Task<CreatedApplicationUser?> CreateUser(string userName, string email, string password);

        Task<string> GenerateRegistrationTokenAsync(string userId);

        Task<BaseResult> ConfirmUserEmail(string email, string confirmationToken);

        Task<BaseResult> ChangePassword(ChangePasswordAction changePasswordRequest);

        Task Signout();
    }
}
