namespace DietPlanner.Api.DTO.Products
{
    public class ProductDTO: BaseItem
    {
        public ProductDTO()
        {
            ItemType = ItemType.Product;
        }

        public float Carbohydrates { get; set; }

        public float Proteins { get; set; }

        public float Fats { get; set; }

        public float Calories { get; set; }

        public long? BarCode { get; set; }
    }
}
