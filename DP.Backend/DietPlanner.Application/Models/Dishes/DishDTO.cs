using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Models.Dishes;

public class DishDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string ImagePath { get; set; }

    public required string Description { get; set; }

    public ItemType ItemType => ItemType.Dish;

    public bool ExposeToOtherUsers { get; set; }

    public bool IsOwner { get; set; }

    public required IEnumerable<DishProductsDTO> Products { get; set; }
}
