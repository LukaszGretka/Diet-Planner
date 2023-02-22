using DietPlanner.Api.Models;
using DietPlanner.Shared.Models;

namespace DietPlanner.Api.Services.SignUp
{
    public class AccountService : IAccountService
    {
        public Task<DatabaseActionResult<SignUpRequest>> SignUp(SignUpRequest signUpRequestData)
        {
            throw new System.NotImplementedException();
        }

        public Task<LogInResult> LogIn(LogInRequest loginRequest)
        {
            throw new NotImplementedException();
        }

    }
}
