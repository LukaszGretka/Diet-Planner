using DietPlanner.Api.DTO.UserProfile;
using DietPlanner.Api.Extensions;
using DietPlanner.Application.DTO.UserProfile;
using DietPlanner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile()
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            return await userProfileService.GetUserProfile(userId);
        }


        [HttpPatch]
        public async Task<ActionResult<UserProfileDTO>> UpdateUserProfile(UserProfileDTO userProfile)
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var result = await userProfileService.UpdateUserProfile(userId, userProfile);

            return result.Obj;
        }

        [HttpPatch("avatar")]
        public async Task<UserProfileDTO> UploadAvatar(UserAvatarDTO userAvatar)
        {
            var userId = HttpContext.GetUserId();
            var result = await userProfileService.UploadAvatar(userId, userAvatar.Base64Image);

            return result.Obj;
        }
    }
}
