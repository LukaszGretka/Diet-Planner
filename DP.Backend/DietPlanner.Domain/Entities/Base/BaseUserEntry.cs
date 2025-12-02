using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities.Base
{
    public class BaseUserEntity
    {
        [Key]
        public required string UserId { get; set; }
    }
}
