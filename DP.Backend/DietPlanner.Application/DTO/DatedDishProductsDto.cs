using DietPlanner.Application.DTO.Dishes;

namespace DietPlanner.Application.DTO
{
    public class DatedDishProductsDto
    {
        public DateTime Date { get; set; }

        public required IEnumerable<DishProductsDTO> DishProducts { get; set; }
    }
}
