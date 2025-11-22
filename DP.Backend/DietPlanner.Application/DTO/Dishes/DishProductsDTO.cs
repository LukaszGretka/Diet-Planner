using DietPlanner.Domain.Entities.Products;

namespace DietPlanner.Application.DTO.Dishes
{
    public class DishProductsDTO
    {
        public int DishProductId { get; set; }

        public required Product Product { get; set; }

        public decimal PortionMultiplier { get; set; }

        private decimal? _customizedPortionMultiplier;

        public decimal? CustomizedPortionMultiplier
        {
            get => _customizedPortionMultiplier;
            set => _customizedPortionMultiplier = value <= 0 ? null : value;
        }
    }
}
