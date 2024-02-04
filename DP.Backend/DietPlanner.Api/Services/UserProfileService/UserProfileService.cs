using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.UserProfile;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.UserProfileService
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IdentityDatabaseContext _databaseContext;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(IdentityDatabaseContext databaseContext, ILogger<UserProfileService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<UserProfileDTO> GetUserProfile(string userId)
        {
            var userProfile = await _databaseContext.UserProfile.FindAsync(userId);

            if (userProfile is null)
            {
                _logger.LogWarning("User not found during getting user profile");
            }

            return new UserProfileDTO
            {
                Name = userProfile.Name?.Trim(),
                Gender = (int)userProfile.Gender,
                BirthDate = userProfile.BirthDate,
                Height = userProfile.Height
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
                        Height = userProfile.Height
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
                    Height = userProfile.Height
                });

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<UserProfileDTO>(false, exception: ex);
            }
        }
    }
}
