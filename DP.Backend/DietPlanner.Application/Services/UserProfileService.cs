using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.UserProfile;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DietPlanner.Application.Services;

public class UserProfileService(ILogger<UserProfileService> logger, IUserProfileRepository userProfileRepository) : IUserProfileService
{
    public async Task<UserProfileDTO?> GetUserProfile(string userId, CancellationToken ct)
    {
        UserProfile? userProfile = await userProfileRepository.GetByIdAsync(userId, ct);

        if (userProfile is null)
        {
            logger.LogWarning("User not found during getting user profile");
            return null;
        }

        return new UserProfileDTO
        {
            Name = userProfile.Name?.Trim() ?? string.Empty,
            Gender = (int)userProfile.Gender,
            BirthDate = userProfile.BirthDate,
            Height = userProfile.Height,
            Base64Image = Encoding.UTF8.GetString(userProfile.Avatar ?? Array.Empty<byte>())
        };
    }

    public async Task<UserProfileDTO?> AddUserProfile(string userId, UserProfileDTO userProfileDTO, CancellationToken ct)
    {
        UserProfile? userProfile = await userProfileRepository.GetByIdAsync(userId, ct);

        if (userProfile is not null)
        {
            logger.LogInformation("User profile already exists");
            return null;
        }

        UserProfile newUserProfile = new()
        {
            UserId = userId,
            Name = userProfileDTO.Name,
            BirthDate = userProfileDTO.BirthDate,
            Gender = (GenderType)userProfileDTO.Gender,
            Height = userProfileDTO.Height
        };

        UserProfile createdUser = await userProfileRepository.CreateAsync(newUserProfile, ct);

        if (createdUser is null)
        {
            logger.LogInformation("Unable to create user profile");
            return null;
        }

        return new()
        {
            Name = createdUser.Name, 
            Gender = (int)createdUser.Gender,
            BirthDate = createdUser.BirthDate,
            Height = createdUser.Height,
            Base64Image = Encoding.UTF8.GetString(createdUser?.Avatar ?? [])
        };
    }

    public async Task<UserProfileDTO?> UpdateUserProfile(string userId, UserProfileDTO userProfileDTO, CancellationToken ct)
    {
        UserProfile? userProfile = await userProfileRepository.GetByIdAsync(userId, ct);

        if (userProfile is null)
        {
            logger.LogWarning("User not found during updating user profile");
            return null;
        }

        userProfile.Name = userProfileDTO.Name;
        userProfile.Gender = (GenderType)userProfileDTO.Gender;
        userProfile.BirthDate = userProfileDTO.BirthDate;
        userProfile.Height = userProfileDTO.Height;

        UserProfile updatedUserProfile = await userProfileRepository.UpdateAsync(userProfile, ct);

        return new()
        {
            Name = updatedUserProfile.Name,
            Gender = (int)updatedUserProfile.Gender,
            BirthDate = updatedUserProfile.BirthDate,
            Height = userProfile.Height,
            Base64Image = Encoding.UTF8.GetString(userProfile?.Avatar ?? [])
        };
    }

    public async Task<UserProfileDTO?> UploadAvatar(string userId, string base64Avatar, CancellationToken ct)
    {
        UserProfile? userProfile = await userProfileRepository.GetByIdAsync(userId, ct);

        if (userProfile is null)
        {
            logger.LogWarning("User not found during updating user's avatar");
            return null;
        }

        userProfile.Avatar = Encoding.UTF8.GetBytes(base64Avatar);

        UserProfile updatedUserProfile = await userProfileRepository.UpdateAsync(userProfile, ct);

        if (userProfile is null)
        {
            logger.LogWarning("Unable to upload user's avatar");
            return null;
        }

        return new()
        {
            Name = updatedUserProfile.Name,
            Gender = (int)updatedUserProfile.Gender,
            BirthDate = updatedUserProfile.BirthDate,
            Height = updatedUserProfile.Height,
            Base64Image = Encoding.UTF8.GetString(updatedUserProfile.Avatar ?? [])
        };
    }
}
