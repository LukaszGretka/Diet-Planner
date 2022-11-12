using DietPlanner.Identity.Models;
using DietPlanner.Shared.Models;

namespace DietPlanner.Identity.Services.SignUp
{
    public interface ISignUpService
    {
        public Task<DatabaseActionResult<SignUpRequest>> CreateUser(SignUpRequest signUpRequestData);
    }
}
