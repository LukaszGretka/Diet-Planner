using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Models.Goal
{
    public class GoalDTO
    {
        public float Value { get; set; }

        public DateTime EstablishmentDate { get; set; }

        public GoalType GoalType { get; set; }
    }
}
