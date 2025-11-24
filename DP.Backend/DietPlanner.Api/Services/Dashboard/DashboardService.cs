using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Models.Dashboard;
using DietPlanner.Api.Services.MealProductService;
using DietPlanner.Domain.Enums;
using DietPlanner.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IMeasurementService _measurementService;
        private readonly IMealService _mealService;
        private readonly DietPlannerDbContext _databaseContext;
        private readonly IGoalService _goalService;

        public DashboardService(IMeasurementService measurementService, IMealService mealService, IGoalService goalService, DietPlannerDbContext databaseContext)
        {
            _measurementService = measurementService;
            _mealService = mealService;
            _databaseContext = databaseContext;
            _goalService = goalService;
        }

        public async Task<DashboardData> GetDashboardData(string userId)
        {
            var measurements = await _measurementService.GetAll(userId);
            decimal? currentWeight = measurements.LastOrDefault()?.Weight;

            var goal = await _goalService.GetGoalData(userId, GoalType.CaloricDemand);
            int caloricDemand = (int)goal.Value;

            var dataTimeNow = DateTime.Now.Date;

            List<DatedDishProductsDto> datedDishProducts = _databaseContext.Meals
                .Join(_databaseContext.MealDishes, m => m.Id, md => md.MealId, (m, md) => new {m.Date, m.UserId, md.DishId, mealDishId = md.Id})
                    .Where(m => m.UserId == userId && (m.Date >= dataTimeNow.AddDays(-7) && m.Date <= dataTimeNow))
                    .GroupBy(m => new { m.Date })
                    .Select(gd => new DatedDishProductsDto
                    {
                        Date = gd.Key.Date,
                        DishProducts = gd.Join(_databaseContext.DishProducts, x => x.DishId, dp => dp.DishId, (x, dp) => new { x.DishId, x.mealDishId, dp})
                        .Join(_databaseContext.Products, x => x.dp.ProductId, p => p.Id, (x, p) => new DishProductsDTO
                        {
                            Product = p,
                            PortionMultiplier = x.dp.PortionMultiplier,
                            CustomizedPortionMultiplier = _databaseContext.CustomizedMealDishes
                                .Where(e => e.MealDishId == x.mealDishId && e.DishProductId == x.dp.Id)
                                .SingleOrDefault().CustomizedPortionMultiplier
                        })
                    }).ToList();

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
