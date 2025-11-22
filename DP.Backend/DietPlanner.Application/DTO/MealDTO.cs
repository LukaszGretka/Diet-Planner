using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Application.DTO.Products;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.DTO
{
    public class MealDto
    {
        public MealType MealType { get; set; }

        public required List<DishDTO> Dishes { get; set; }

        public required List<ProductDTO> Products { get; set; }
    }

    public class PutMealRequest : MealDto
    {
        public DateTime Date { get; set; }
    }

    public class MealProductDto
    {
        public DateTime Date { get; set; }

        public MealType MealType { get; set; }

        public required ProductDTO Product { get; set; }
    }
}
