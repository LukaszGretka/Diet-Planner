using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
