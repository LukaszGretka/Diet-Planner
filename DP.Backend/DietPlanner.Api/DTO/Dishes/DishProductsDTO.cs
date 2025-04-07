using DietPlanner.Api.Models.MealsCalendar.DbModel;

namespace DietPlanner.Api.DTO.Dishes
{
    public class DishProductsDTO
    {
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
