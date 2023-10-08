using DietPlanner.Api.Models.MealsCalendar.DbModel;

namespace DietPlanner.Api.DTO.Dishes
{
    public class DishProductsDTO
    {
        public Product Product { get; set; }

        public decimal PortionMultiplier { get; set; }

        public decimal CustomizedPortionMultiplier { get; set; }
    }
}
