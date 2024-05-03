using DietPlanner.Api.DTO.UserProfile;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.UserProfileService
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> GetUserProfile(string userId);

        Task<DatabaseActionResult<UserProfileDTO>> UpdateUserProfile(string userId, UserProfileDTO userProfileDTO);

        Task<DatabaseActionResult<UserProfileDTO>> UploadAvatar(string userId, string base64Avatar);
    }
}
