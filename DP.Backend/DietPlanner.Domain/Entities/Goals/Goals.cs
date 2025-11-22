using DietPlanner.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities.Goals
{
    public class Goals
    {
        [Key]
        public string UserId { get; set; }

        public float Value { get; set; }

        public DateTime EstablishmentDate { get; set; }

        public GoalType GoalType { get; set; }
    }
}