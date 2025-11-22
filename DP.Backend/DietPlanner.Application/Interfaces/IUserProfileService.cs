using DietPlanner.Api.DTO.UserProfile;

namespace DietPlanner.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> GetUserProfile(string userId);

        Task<DatabaseActionResult<UserProfileDTO>> UpdateUserProfile(string userId, UserProfileDTO userProfileDTO);

        Task<DatabaseActionResult<UserProfileDTO>> UploadAvatar(string userId, string base64Avatar);
    }
}
