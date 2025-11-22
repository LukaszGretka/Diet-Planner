using DietPlanner.Application.DTO.Dashboard;
using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Services
{
    public class DashboardService(IMeasurementService measurementService, 
        IGoalService goalService, 
        IDashboardRepository dashboardRepository) : IDashboardService
    {
        public async Task<DashboardData> GetDashboardData(string userId)
        {
            var measurements = await measurementService.GetAll(userId);
            decimal? currentWeight = measurements.LastOrDefault()?.Weight;

            var goal = await goalService.GetGoalData(userId, GoalType.CaloricDemand);
            int caloricDemand = (int)goal.Value;

            var dataTimeNow = DateTime.Now.Date;

            var datedDishProducts = await dashboardRepository.GetDatedDishProducts(userId, dataTimeNow);

            List<float> caloriesLastSevenDays = new(7);
            List<float> carbsLastSevenDays = new(7);
            List<float> proteinsLastSevenDays = new(7);
            List<float> fatsLastSevenDays = new(7);

            for (int i = 0; i < 7; i++)
            {
                var existingDishProductOnDate = datedDishProducts
                    .Where(ddp => ddp.Date.Date == dataTimeNow.AddDays(-i)).Select(e => e.DishProducts)
                    .FirstOrDefault();

                if(existingDishProductOnDate is not null)
                {
                    caloriesLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product.Calories, dp)));
                    carbsLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product.Carbohydrates, dp)));
                    proteinsLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product.Proteins, dp)));
                    fatsLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product.Fats, dp)));
                }
                else
                {
                    caloriesLastSevenDays.Add(0f);
                    carbsLastSevenDays.Add(0f);
                    proteinsLastSevenDays.Add(0f);
                    fatsLastSevenDays.Add(0f);
                }
            }

            caloriesLastSevenDays.Reverse();
            carbsLastSevenDays.Reverse();
            proteinsLastSevenDays.Reverse();
            fatsLastSevenDays.Reverse();

            return new DashboardData
            {
                CurrentWeight = (float?)currentWeight,
                CaloriesLastSevenDays = caloriesLastSevenDays.ToArray(),
                CarbsLastSevenDays = carbsLastSevenDays.ToArray(),
                ProteinsLastSevenDays = proteinsLastSevenDays.ToArray(),
                FatsLastSevenDays = fatsLastSevenDays.ToArray(),
                CaloricDemand = caloricDemand,
            };
        }

        private static float CalculateMultiplierValue(float? stat, DishProductsDTO dishProductDTO)
        {
           var result = (stat * (float)(dishProductDTO.CustomizedPortionMultiplier ?? dishProductDTO.PortionMultiplier)) ?? 0f;
            return result;
        }
    }
}
