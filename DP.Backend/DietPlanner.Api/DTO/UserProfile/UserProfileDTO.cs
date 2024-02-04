using System;

namespace DietPlanner.Api.DTO.UserProfile
{
    public class UserProfileDTO
    {
        public string Name { get; set; }

        public int Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }
    }
}
