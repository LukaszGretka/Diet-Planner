using DietPlanner.Api.Models;
using DietPlanner.Shared.Models;

namespace DietPlanner.Api.Services.SignUp
{
    public interface IAccountService
    {
        public Task<DatabaseActionResult<SignUpRequest>> SignUp(SignUpRequest 
            signUpRequestData);

        public Task<LogInResult> LogIn(LogInRequest loginRequest);
    }
}
