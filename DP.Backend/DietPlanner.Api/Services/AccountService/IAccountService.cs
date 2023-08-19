using DietPlanner.Api.Models.Account;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.AccountService
{
    public interface IAccountService
    {
        public Task<IdentityUser> GetUser(string email);

        public Task<DatabaseActionResult<SignUpResponse>> SignUp(SignUpRequest signUpRequestData);

        public Task<SignInResult> SignIn(HttpContext httpContext, SignInRequest loginRequest);

        public Task Logout();

        public Task<IdentityResult> ConfirmUserEmail(EmailConfirmationRequest activateAccountRequest);
    }
}
