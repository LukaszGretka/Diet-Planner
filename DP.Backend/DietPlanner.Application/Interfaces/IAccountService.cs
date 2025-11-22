using DietPlanner.Application.DTO.Identity;
using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Application.Interfaces
{
    public interface IAccountService
    {
        public Task<IdentityUser?> GetUser(string userName);

        public Task<SignupResult> SignUp(string userName, string email, string password);

        public Task<SignInResult> SignIn(string userName, string password);

        public Task Logout();

        public Task<IdentityResult> ConfirmUserEmail(string email, string confirmationToken);

        public Task<IdentityResult> ChangePassword(string currentPassword, string newPassword, string newConfirmedPassword, string userName);
    }
}
