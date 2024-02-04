using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.UserProfile;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Services.UserProfileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile()
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            return await _userProfileService.GetUserProfile(userId);
        }


        [HttpPatch]
        public async Task<ActionResult<UserProfileDTO>> UpdateUserProfile(UserProfileDTO userProfile)
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var result = await _userProfileService.UpdateUserProfile(userId, userProfile);

            return result.Obj;
        }
    }
}
