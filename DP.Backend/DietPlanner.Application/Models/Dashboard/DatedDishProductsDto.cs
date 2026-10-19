using DietPlanner.Application.Models.Dishes;

namespace DietPlanner.Application.Models.Dashboard;

public class DatedDishProductsDto
{
    public DateTime Date { get; set; }
    public required IEnumerable<DishProductsDTO> DishProducts { get; set; }
}
