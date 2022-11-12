using DietPlanner.Identity.Models;
using DietPlanner.Shared.Models;

namespace DietPlanner.Identity.Services.SignUp
{
    public class SignUpService : ISignUpService
    {
        public Task<DatabaseActionResult<SignUpRequest>> CreateUser(SignUpRequest signUpRequestData)
        {
            throw new System.NotImplementedException();
        }
    }
}
