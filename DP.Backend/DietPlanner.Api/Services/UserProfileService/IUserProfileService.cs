using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.UserProfile;
using DietPlanner.Shared.Models;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.UserProfileService
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> GetUserProfile(string userId);

        Task<DatabaseActionResult<UserProfileDTO>> UpdateUserProfile(string userId, UserProfileDTO userProfileDTO);
    }
}
