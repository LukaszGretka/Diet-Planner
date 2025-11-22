using DietPlanner.Api.DTO.UserProfile;
using DietPlanner.Application.Interfaces;
using DietPlanner.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DietPlanner.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly DietPlannerDbContext _databaseContext;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(DietPlannerDbContext databaseContext, ILogger<UserProfileService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<UserProfileDTO?> GetUserProfile(string userId)
        {
            UserProfile userProfile = await _databaseContext.UserProfile.FindAsync(userId);

            if (userProfile is null)
            {
                _logger.LogWarning("User not found during getting user profile");
                return null;
            }

            return new UserProfileDTO
            {
                Name =  userProfile.Name?.Trim(),
                Gender = (int)userProfile.Gender,
                BirthDate = userProfile.BirthDate,
                Height = userProfile.Height,
                Base64Image = Encoding.UTF8.GetString(userProfile.Avatar ?? Array.Empty<byte>())
            };
        }

        public async Task<DatabaseActionResult<UserProfileDTO>> UpdateUserProfile(string userId, UserProfileDTO userProfileDTO)
        {
            try
            {
                var userProfile = await _databaseContext.UserProfile.FindAsync(userId);

                if (userProfile is null)
                {
                    var newUserProfile = new UserProfile
                    {
                        UserId = userId,
                        Name = userProfileDTO.Name,
                        BirthDate = userProfileDTO.BirthDate,
                        Gender = (GenderType)userProfileDTO.Gender,
                        Height = userProfileDTO.Height
                    };

                    await _databaseContext.AddAsync(newUserProfile);
                    await _databaseContext.SaveChangesAsync();

                    return new DatabaseActionResult<UserProfileDTO>(true, obj: new UserProfileDTO
                    {
                        Name = userProfile.Name,
                        Gender = (int)userProfile.Gender,
                        BirthDate = userProfile.BirthDate,
                        Height = userProfile.Height,
                        Base64Image = Encoding.UTF8.GetString(userProfile?.Avatar ?? Array.Empty<byte>())
                    });
                }

                userProfile.Name = userProfileDTO.Name;
                userProfile.Gender = (GenderType)userProfileDTO.Gender;
                userProfile.BirthDate = userProfileDTO.BirthDate;
                userProfile.Height = userProfileDTO.Height;

                await _databaseContext.SaveChangesAsync();

                return new DatabaseActionResult<UserProfileDTO>(true, obj: new UserProfileDTO
                {
                    Name = userProfile.Name,
                    Gender = (int)userProfile.Gender,
                    BirthDate = userProfile.BirthDate,
                    Height = userProfile.Height,
                    Base64Image = Encoding.UTF8.GetString(userProfile?.Avatar ?? Array.Empty<byte>())
                });

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<UserProfileDTO>(false, exception: ex);
            }
        }

        public async Task<DatabaseActionResult<UserProfileDTO>> UploadAvatar(string userId, string base64Avatar)
        {
            UserProfile userProfile = await _databaseContext.UserProfile.FindAsync(userId);

            if (userProfile is null)
            {
                _logger.LogWarning("User not found during updating user's avatar");
                return null;
            }

            userProfile.Avatar = Encoding.UTF8.GetBytes(base64Avatar);
            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult<UserProfileDTO>(true, obj: new UserProfileDTO
            {
                Name = userProfile.Name,
                Gender = (int)userProfile.Gender,
                BirthDate = userProfile.BirthDate,
                Height = userProfile.Height,
                Base64Image = Encoding.UTF8.GetString(userProfile.Avatar)
            });
        }
    }
}
