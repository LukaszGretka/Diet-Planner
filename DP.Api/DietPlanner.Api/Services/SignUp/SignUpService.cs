using DietPlanner.Api.Models;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.SignUp
{
    public class SignUpService : ISignUpService
    {
        public Task<DatabaseActionResult<SignUpRequest>> CreateUser(SignUpRequest signUpRequestData)
        {
            throw new System.NotImplementedException();
        }
    }
}
