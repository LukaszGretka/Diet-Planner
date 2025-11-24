using DietPlanner.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities
{
    public class Goals
    {
        [Key]
        public string? UserId { get; set; }

        public float Value { get; set; }

        public DateTime EstablishmentDate { get; set; }

        public GoalType GoalType { get; set; }
    }
}