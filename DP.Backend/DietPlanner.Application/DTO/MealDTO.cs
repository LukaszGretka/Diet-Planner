using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Application.DTO.Products;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Api.Models.MealsCalendar.DTO
{
    public class MealDto
    {
        public MealType MealType { get; set; }

        public List<DishDTO> Dishes { get; set; }

        public List<ProductDTO> Products { get; set; }
    }

    public class PutMealRequest : MealDto
    {
        public DateTime Date { get; set; }
    }

    public class MealProductDto
    {
        public DateTime Date { get; set; }

        public MealType MealType { get; set; }

        public ProductDTO Product { get; set; }
    }
}
