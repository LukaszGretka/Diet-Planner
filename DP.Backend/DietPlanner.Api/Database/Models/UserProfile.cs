using System;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Database.Models
{
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; }

        public string Name { get; set; }

        public GenderType Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public byte[] Avatar { get; set; }
    }

    public enum GenderType 
    { 
        Unknown = 0,
        Male = 1,
        Female = 2,
        Other = 3,
    }
}
