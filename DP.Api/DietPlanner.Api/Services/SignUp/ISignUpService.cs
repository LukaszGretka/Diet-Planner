using DietPlanner.Api.Models;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.SignUp
{
    public interface ISignUpService
    {
        public Task<DatabaseActionResult<SignUpRequest>> CreateUser(SignUpRequest signUpRequestData);
    }
}
