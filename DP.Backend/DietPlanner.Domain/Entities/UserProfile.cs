using DietPlanner.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities
{
    public class UserProfile
    {
        [Key]
        public string? UserId { get; set; }

        public string? Name { get; set; }

        public GenderType Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public byte[]? Avatar { get; set; }
    }
}
