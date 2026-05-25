using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}