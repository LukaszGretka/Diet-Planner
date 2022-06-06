using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Models
{
    public class MealType
    {
        [Key]
        public int Id { get; set; }
        public string MealName { get; set; }
    }
}
