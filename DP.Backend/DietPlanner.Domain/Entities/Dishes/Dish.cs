namespace DietPlanner.Domain.Entities.Dishes
{
    public class Dish : BaseEntity
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public bool ExposeToOtherUsers { get; set; }
    }
}
