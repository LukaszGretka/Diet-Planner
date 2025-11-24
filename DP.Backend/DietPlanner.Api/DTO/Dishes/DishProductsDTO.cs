using DietPlanner.Domain.Entities;

namespace DietPlanner.Api.DTO.Dishes
{
    public class DishProductsDTO
    {
        public int DishProductId { get; set; }

        public Product Product { get; set; }

        public decimal PortionMultiplier { get; set; }

        private decimal? _customizedPortionMultiplier;

        public decimal? CustomizedPortionMultiplier
        {
            get => _customizedPortionMultiplier;
            set => _customizedPortionMultiplier = value <= 0 ? null : value;
        }
    }
}
