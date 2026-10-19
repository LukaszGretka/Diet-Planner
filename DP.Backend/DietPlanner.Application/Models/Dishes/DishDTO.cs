using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Models.Dishes;

public class DishDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? ImagePath { get; set; }

    public string? Description { get; set; }

    public ItemType ItemType => ItemType.Dish;

    public bool ExposeToOtherUsers { get; set; }

    public bool IsOwner { get; set; }

    public required IEnumerable<DishProductsDTO> Products { get; set; }
}
