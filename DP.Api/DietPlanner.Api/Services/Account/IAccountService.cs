using DietPlanner.Api.Models.Account;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Account
{
    public interface IAccountService
    {
        public Task<IdentityUser> GetUser(string email);

        public Task<DatabaseActionResult<IdentityUser>> SignUp(SignUpRequest signUpRequestData);

        public Task<SignInResult> LogIn(HttpContext httpContext, LogInRequest loginRequest);

        public Task Logout();
    }
}
