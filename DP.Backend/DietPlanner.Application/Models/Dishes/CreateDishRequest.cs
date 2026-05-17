namespace DietPlanner.Application.Models.Dishes;

public class CreateDishRequest
{
    public required string Name { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public required ICollection<DishProductsDTO> Products { get; set; }

    public bool ExposeToOtherUsers { get; set; }
}
