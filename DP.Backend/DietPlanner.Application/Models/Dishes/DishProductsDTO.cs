using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Models.Dishes;

public class DishProductsDTO
{
    public int DishProductId { get; set; }

    public Product Product { get; set; }

    public decimal PortionMultiplier { get; set; }

    public decimal? CustomizedPortionMultiplier
    {
        get;
        set => field = value <= 0 ? null : value;
    }
}
