using DietPlanner.Application.Models.Account;
using DietPlanner.Domain.Entities.Account;
using DietPlanner.Domain.Entities.Results;
using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Application.Interfaces.Adapters;

public interface IAccountManagerAdapter
{
    Task<ApplicationUser?> GetUserByName(string userName);

    Task<ApplicationUser?> GetUserByEmail(string email);

    Task<SignInResult> PasswordSignInAsync(string username, string password);

    Task Signout();

    Task<CreatedApplicationUser?> CreateUser(string userName, string email, string password);

    Task<BaseResult> ConfirmUserEmail(string email, string confirmationToken);

    Task<string> GenerateRegistrationTokenAsync(string userId);

    Task<BaseResult> ChangePassword(ChangePasswordAction changePasswordRequest);
}
