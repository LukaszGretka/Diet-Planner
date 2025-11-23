using DietPlanner.Application.Models;
using DietPlanner.Application.Models.Account;
using DietPlanner.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser?> GetUser(string userName);

        Task<SignInResult> SignIn(string userName, string password);

        Task<SignUpResult> SignUp(string userName, string email, string password);

        Task Logout();

        Task<BaseResult> ConfirmUserEmail(string email, string confirmationToken);

        Task<BaseResult> ChangePassword(ChangePasswordAction changePasswordRequest);
    }
}
