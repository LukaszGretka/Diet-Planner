using DietPlanner.Application.Models.UserProfile;

namespace DietPlanner.Application.Interfaces.Services;

public interface IUserProfileService
{
    Task<UserProfileDTO?> GetUserProfile(string userId, CancellationToken ct);

    Task<UserProfileDTO?> AddUserProfile(string userId, UserProfileDTO userProfileDTO, CancellationToken ct);

    Task<UserProfileDTO?> UpdateUserProfile(string userId, UserProfileDTO userProfileDTO, CancellationToken ct);

    Task<UserProfileDTO?> UploadAvatar(string userId, string base64Avatar, CancellationToken ct);
}
