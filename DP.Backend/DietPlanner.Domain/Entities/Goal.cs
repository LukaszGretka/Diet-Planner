using DietPlanner.Domain.Entities.Base;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Domain.Entities
{
    public class Goal: BaseUserEntity
    {
        public float Value { get; set; }

        public DateTime EstablishmentDate { get; set; }

        public GoalType GoalType { get; set; }
    }
}