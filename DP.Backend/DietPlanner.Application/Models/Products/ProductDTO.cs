using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Models.Products;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public ItemType ItemType => ItemType.Product;
    public float Carbohydrates { get; set; }
    public float Proteins { get; set; }
    public float Fats { get; set; }
    public float Calories { get; set; }
    public long? BarCode { get; set; }
    public decimal? PortionMultiplier { get; set; }
}