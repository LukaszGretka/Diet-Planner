using DietPlanner.Domain.Enums;

namespace DietPlanner.Api.DTO
{
    public class BaseItem
    {
        public int Id { get; set; }

        public int MealItemId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public ItemType ItemType { get; set; }
    }
}
