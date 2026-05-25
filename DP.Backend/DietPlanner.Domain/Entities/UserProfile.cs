using DietPlanner.Domain.Entities.Base;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Domain.Entities
{
    public class UserProfile : BaseUserEntity
    {

        public string? Name { get; set; }

        public GenderType Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public byte[]? Avatar { get; set; }
    }
}
