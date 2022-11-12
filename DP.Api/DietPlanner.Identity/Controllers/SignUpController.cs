using DietPlanner.Identity.Models;
using DietPlanner.Identity.Services.SignUp;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DietPlanner.Identity.Controllers
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

            return Ok();
            //return CreatedAtAction(nameof(PostAsync), new { id = result.Obj.Id }, result.Obj);
        }
    }
}
