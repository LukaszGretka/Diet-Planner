using DietPlanner.Domain.Enums;
using System;

namespace DietPlanner.Api.Requests
{
    public class MealItemRequest
    {
        public int ItemId { get; set; }

        public int MealItemId { get; set; }

        public ItemType ItemType { get; set; }

        public MealType MealType { get; set; }

        public DateTime Date { get; set; }
    }
}
