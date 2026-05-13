using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Models.MealsCalendar
{
    public class MealDishDto
    {
        public int Id { get; set; }
        public int MealItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool ExposeToOtherUsers { get; set; }
        public bool IsOwner { get; set; }
        public ItemType ItemType { get; set; }
        public List<MealDishProductDto> Products { get; set; }
    }

    public class MealDishProductDto
    {
        public int DishProductId { get; set; }
        public MealProductDto Product { get; set; }
        public decimal PortionMultiplier { get; set; }
        public decimal? CustomizedPortionMultiplier { get; set; }
    }

    public class MealProductDto
    {
        public int Id { get; set; }
        public int MealItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ItemType ItemType { get; set; }
        public long? BarCode { get; set; }
        public float Calories { get; set; }
        public float Carbohydrates { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public decimal? PortionMultiplier { get; set; }
    }

    public class MealDto
    {
        public MealType MealType { get; set; }
        public List<MealDishDto> Dishes { get; set; }
        public List<MealProductDto> Products { get; set; }
    }

    public class MealItemRequest
    {
        public int ItemId { get; set; }
        public int MealItemId { get; set; }
        public ItemType ItemType { get; set; }
        public MealType MealType { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateMealItemPortionRequest
    {
        public DateTime Date { get; set; }
        public ItemType ItemType { get; set; }
        public int ItemProductId { get; set; }
        public int? DishProductId { get; set; }
        public decimal CustomizedPortionMultiplier { get; set; }
    }
}
