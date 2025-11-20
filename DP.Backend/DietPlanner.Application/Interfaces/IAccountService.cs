using DietPlanner.Api.Models.Account;
using DietPlanner.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.AccountService
{
    public interface IAccountService
    {
        public Task<IdentityUser> GetUser(string username);

        public Task<DatabaseActionResult<SignUpResponse>> SignUp(SignUpRequest signUpRequestData);

        public Task<SignInResult> SignIn(HttpContext httpContext, SignInRequest loginRequest);

        public Task Logout();

        public Task<IdentityResult> ConfirmUserEmail(EmailConfirmationRequest activateAccountRequest);

        public Task<IdentityResult> ChangePassword(ChangePasswordRequest changePasswordRequest, IIdentity identity);
    }
}
