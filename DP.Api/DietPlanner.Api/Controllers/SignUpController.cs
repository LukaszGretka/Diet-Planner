using DietPlanner.Api.Models;
using DietPlanner.Api.Services.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    public class SignUpController : Controller
    {
        private readonly ISignUpService _signUpService;

        public SignUpController(ISignUpService signUpService)
        {
            _signUpService = signUpService;
        }

        [HttpPost]
        [ActionName(nameof(PostAsync))]
        public async Task<IActionResult> PostAsync([FromBody] SignUpRequest signUpRequest)
        {
            DatabaseActionResult<SignUpRequest> result = await _signUpService.CreateUser(signUpRequest);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(PostAsync), new { id = result.Obj.Id }, result.Obj);
        }
    }
}
