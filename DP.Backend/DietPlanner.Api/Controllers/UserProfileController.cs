using DietPlanner.Api.Extensions;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Models.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
    {
        private readonly IUserProfileService _userProfileService = userProfileService;

        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var userProfile = await _userProfileService.GetUserProfile(userId, ct);

            if (userProfile is null)
            {
                return NotFound();
            }

            return userProfile;
        }


        [HttpPatch]
        public async Task<ActionResult<UserProfileDTO>> UpdateUserProfile(UserProfileDTO userProfile, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            bool isUserProfileExists = (await _userProfileService.GetUserProfile(userId, ct)) is not null;

            UserProfileDTO result = isUserProfileExists ? await _userProfileService.UpdateUserProfile(userId, userProfile, ct)
                : await _userProfileService.AddUserProfile(userId, userProfile, ct);

            if (result is null) 
            {
                return NotFound();
            }

            return result;
        }

        [HttpPatch("avatar")]
        public async Task<ActionResult<UserProfileDTO>> UploadAvatar(UserAvatarDTO userAvatar, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            UserProfileDTO result = await _userProfileService.UploadAvatar(userId, userAvatar.Base64Image, ct);

            if (result is null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
